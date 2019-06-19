using System.Threading.Tasks;
using System;
using CoEco.Core.Ordering.Dto.Responses;
using CoEco.Front.Auth.Domain;
using CoEco.Core.Ordering.Domain;
using System.Configuration;
using CoEco.Core.Services;
using static CoEco.Front.Auth.Domain.AuthError;
using CoEco.Core.Infrastructure;

namespace CoEco.Front.Auth.Services
{

    public class AuthenticationService : IAuthenticationService
    {
        private static Random rnd = new Random();

        private readonly ILoginService loginResultService;
        private readonly IAppUserService userService;
        private readonly IVerificationService verificationService;
        private readonly ISmsSender smsService;

        public AuthenticationService(
            ILoginService loginResultService,
            IAppUserService userService,
            IVerificationService verificationService,
            ISmsSender smsService)
        {
            this.loginResultService = loginResultService;
            this.userService = userService;
            this.verificationService = verificationService;
            this.smsService = smsService;
        }

        public async Task<Result<string>> CreateCode(string username)
        {
            var user = await userService.GetUser(username);
            if (user == null)
                return UserNotFound;

            var code = GenerateCode();
            var sendSuccess = smsService.Send(user.Phone, code);
            if (sendSuccess != SmsResult.OK)
            {
                return FailedToSendSms;
            }

            await verificationService.Create(username, code);

            return code;
        }

        public async Task<Result<User>> Authenticate(string username, string code)
        {
            var error = await GetErrors(username, code);

            if (error != null)
            {
                await loginResultService.SaveResult(username, false);
                return error;
            }

            await verificationService.SetVerified(username, code);
            await loginResultService.SaveResult(username, true);

            var user = await userService.GetUser(username);
            return user;
        }

        public async Task<Error> GetErrors(string username, string code)
        {
            var userExists = await userService.UserExists(username);
            if (!userExists.Success)
                return userExists.Error;

            var numFailed = await loginResultService.GetNumFailed(username);
            if (numFailed > 5)
                return TooManyFails;

            var verified = await verificationService.Verify(username, code);
            if (!verified)
                return InvalidCode;

            return null;
        }

        private static string GenerateCode()
        {

            var verCode = ConfigurationManager.AppSettings["auth:VerificationCode"];

            if (string.IsNullOrEmpty(verCode))
            {
                return RandomCode();
            }

            return verCode;

        }

        private static string RandomCode()
        {
            return rnd.Next(1000, 9999).ToString();
        }

    }
}

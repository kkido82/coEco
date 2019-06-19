using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using CoEco.BO.Config;
using CoEco.BO.Services;

namespace CoEco.BO.Auth
{
    public class ApplicationUserManager: UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
      : base(store)
        {
        }
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new CoEcoUserValidator(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new CoEcoPasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,

            };


            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = AppAuthConfiguration.Config.UserLockoutEnabledByDefault;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(AppAuthConfiguration.Config.DefaultAccountLockoutTimeSpan);
            manager.MaxFailedAccessAttemptsBeforeLockout = AppAuthConfiguration.Config.MaxFailedAccessAttemptsBeforeLockout;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }


        public override async Task<ApplicationUser> FindAsync(string userName, string password)
        {
            var user = await base.FindAsync(userName, password);

            if (user == null)
            {
                throw new Exception("çùáåï ìà ÷ééí àå ñéñîä ùâåéä");
            }

            if (user.Disabled || IsExpired(user))
            {
                throw new Exception("çùáåðê ðòåì å/àå ìà áúå÷ó");
            }

            return user;
        }

        public bool IsExpired(ApplicationUser user)
        {
            return user.AccountExpiresAt.HasValue && user.AccountExpiresAt.Value.AddDays(1) < DateTime.Now;
        }
    }
}
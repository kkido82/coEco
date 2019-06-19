
using CoEco.Core.Infrastructure;

namespace CoEco.Front.Auth.Domain
{
    public class AuthError
    {
        public static readonly Error UserNotFound = new Error("Auth_UserNotFound", "User not found");
        public static readonly Error InvalidCode = new Error("Auth_InvalidCode", "Invalid code");
        public static readonly Error TooManyFails = new Error("Auth_TooManyFails", "Too Many Fails");
        public static readonly Error FailedToSendSms = new Error("Auth_FailedToSendSms", "Failed To Send Sms");
        public static readonly Error ConnectionPoolIsFull = new Error("Auth_ConnectionPoolIsFull", "Connection Pool Is Full");
    }
}

using System.Collections.Generic;
using System.Linq;

namespace CoEco.Core.Services
{
    public interface ISmsSender
    {
        List<SmsResult> Send(List<SmsItem> smsItems);
        string Provider { get; }
    }

    public enum SmsResult
    {
        OK = 1,
        Failed = -1, // 255 or 257 in cellcom
        BadUserNameOrPassword = -2,//130 in cellcom
        UserNameNotExists = -3,
        PasswordNotExists = -4,
        NoRecipients = -6,
        MessageTextNotExists = -9,
        UserQuotaExceeded = -13,
        ProjectQuotaExceeded = -14,
        CustomerQuotaExceeded = -15,
        WrongDateTime = -16,
        WrongNumberParameter = -17,//242 in cellcom
        WrongRecipients = -18, //11 in cellcom
        InvalidSenderNumber = -20,
        InvalidSenderName = -21,
        UserBlocked = -22,//247 in cellcom
        InvalidPriorityFlag = -100, //6 in cellcom
        MessageReplacementFailed = -101, // 19 in cellcom
        MessageQueueFull = -102, //20  in cellcom
        InvalidReplaceIfPresentFlag = -103,// 84 in cellcom - should not happen
        InvalidValidtyPeriodValue = -104, //98 in cellcom - should not happen
        MissingParameters = -106, // 240 in cellcom
        BadTargetCellcomNumber = -107, // 241 in cellcom - should not happen
        NotAuthorizedForSMS = -109, // 243 in cellcom
        TargetLockedOrLowPrepaidBalance = -110, // 244 in cellcom - should not happen
        TargetDoesNotSupportWap = -111, // 245 in cellcom - should not happen
        BinaryMarketingMessageIsNotSupported = -112, //246 in cellcom - should not happen
        MarketingMessageWithMissingOrIllegalSenderNameOrPhone = -114, // 248 in cellcom
        MarketingMessageTruncatedByMaxSegments = -115, // 249 in cellcom - should not happen
        SMPPResponseTimeout = -117, // 256 in cellcom

    }

    public class SendItem
    {
        public int ItemId { get; set; }
    }
    public class SmsItem : SendItem
    {
        public string message;
        public string phone;
        public SmsResult result;

    }

    public static class ISmsSenderExt
    {
        public static SmsResult Send(this ISmsSender sender, string phone, string msg)
        {
            var item = new SmsItem
            {
                message = msg,
                phone = phone
            };
            var lst = new List<SmsItem>() { item };
            return sender.Send(lst).First();
        }
    }
}

using CoEco.Core.Eventing;
using CoEco.Core.Services;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace CoEco.Services.Services.SmsProviders
{
    public class CellcomSender : ISmsSender
    {
        private readonly ILogger logger;

        public string Provider => "Cellcom";

        public CellcomSender(ILogger logger)
        {
            this.logger = logger;
        }

        public List<SmsResult> Send(List<SmsItem> smsItems)
        {
            var SmsGate = new CellcomSmsGate.SmsGate2();
            SmsGate.Url = ConfigurationManager.AppSettings["SMS-cellcom_url"];
            var username = ConfigurationManager.AppSettings["SMS-cellcom_username"];
            var password = ConfigurationManager.AppSettings["SMS-cellcom_password"];
            var senderName = ConfigurationManager.AppSettings["SMS-cellcom_SenderName"];
            var senderNumber = ConfigurationManager.AppSettings["SMS-cellcom_SenderPhoneNumber"];
            var result = new List<SmsResult>();
            foreach (var smsItem in smsItems)
            {
                //var res = SmsGate.SendSms("idf", "idfsms", smsItem.phone, "mikun|0522199001", "test", null, null, true);

                var code = 0;
                var phone = (smsItem.phone ?? "").Replace("-", "");
                var res = SmsGate.SendSms(username, password, phone, senderName + "|" + senderNumber, smsItem.message, null, null, false);
                if (res.FirstOrDefault() != null && !string.IsNullOrEmpty(res.FirstOrDefault().ErrorDesc))
                {
                    //todo: log
                    //logger.Info("נתקבלה שגיאה מסלקום", "שליחת סמס לסלולרי " + smsItem.phone, details: res.FirstOrDefault().ErrorDesc);
                }
                foreach (var x in res)
                {
                    if (x.Success == false)
                    {
                        code = x.ErrorCode;
                    }
                }
                result.Add(ConvertCellcomCodeToSmsResult(code));
            }

            return result;
        }

        private static SmsResult ConvertCellcomCodeToSmsResult(int code)
        {

            switch (code)
            {
                case 0:
                    return SmsResult.OK;
                case 255:
                case 257:
                    return SmsResult.Failed;
                case 130:
                    return SmsResult.BadUserNameOrPassword;
                case 242:
                    return SmsResult.WrongNumberParameter;
                case 11:
                    return SmsResult.WrongRecipients;
                case 247:
                    return SmsResult.UserBlocked;
                case 6:
                    return SmsResult.InvalidPriorityFlag;
                case 19:
                    return SmsResult.MessageReplacementFailed;
                case 20:
                    return SmsResult.MessageQueueFull;
                case 84:
                    return SmsResult.InvalidReplaceIfPresentFlag;
                case 98:
                    return SmsResult.InvalidValidtyPeriodValue;
                case 240:
                    return SmsResult.MissingParameters;
                case 241:
                    return SmsResult.BadTargetCellcomNumber;
                case 243:
                    return SmsResult.NotAuthorizedForSMS;
                case 244:
                    return SmsResult.TargetLockedOrLowPrepaidBalance;
                case 245:
                    return SmsResult.TargetDoesNotSupportWap;
                case 246:
                    return SmsResult.BinaryMarketingMessageIsNotSupported;
                case 248:
                    return SmsResult.MarketingMessageWithMissingOrIllegalSenderNameOrPhone;
                case 249:
                    return SmsResult.MarketingMessageTruncatedByMaxSegments;
                case 256:
                    return SmsResult.SMPPResponseTimeout;
                default:
                    return SmsResult.Failed;
            }


        }
    }
}

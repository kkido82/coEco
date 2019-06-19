using CoEco.Core.Services;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace CoEco.Services.Services.SmsProviders
{
    public class InforuSender : ISmsSender
    {
        public string Provider => "Inforu";

        public List<SmsResult> Send(List<SmsItem> SmsItems)
        {
            var inforuSmsSender = new InforuSmsGate.SendMessage();
            inforuSmsSender.Url = ConfigurationManager.AppSettings["SMS-Inforu-url"];
            var username = ConfigurationManager.AppSettings["SMS-Inforu-userName"];
            var password = ConfigurationManager.AppSettings["SMS-Inforu-passWord"];
            var senderName = ConfigurationManager.AppSettings["SMS-Inforu-senderName"];
            var senderNumber = ConfigurationManager.AppSettings["SMS-Inforu-senderPhoneNumber"];
            if (SmsItems != null && SmsItems.Any())
            {
                //turn phones to str
                var phoneList = GetPhonesAsString(SmsItems);
                var res = inforuSmsSender.SendSms(username, password, SmsItems.FirstOrDefault().message, phoneList, senderName, senderNumber);
                var s = (SmsResult)int.Parse(XElement.Parse(res).Element("Status").Value);
                var resList = Enumerable.Repeat(s, SmsItems.Count).ToList();
                return resList;
            }
            return new List<SmsResult>();
        }
        private string GetPhonesAsString(List<SmsItem> smsItems)
        {
            var grouping = smsItems.GroupBy(x => x.message + x.message);
            var recivers = new StringBuilder();
            foreach (var group in grouping)
            {
                var body = group.ElementAt(0).message;
                recivers.Append(group.Aggregate(new StringBuilder(), (a, b) =>
                {
                    if (a.Length > 0)
                        a.Append(";");
                    a.Append(b.phone);
                    return a;
                }));

            }
            return recivers.ToString();

        }

    }
}

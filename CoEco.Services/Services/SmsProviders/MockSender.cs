using CoEco.Core.Services;
using System.Collections.Generic;

namespace CoEco.Services.Services.SmsProviders
{
    public class MockSender : ISmsSender
    {
        public string Provider => "Mock";

        public List<SmsResult> Send(List<SmsItem> smsItems)
        {
            var results = new List<SmsResult>();
            smsItems.ForEach(S => results.Add(SmsResult.OK));
            return results;
        }

    }
}

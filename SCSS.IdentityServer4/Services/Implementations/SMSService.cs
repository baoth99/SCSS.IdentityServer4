using SCSS.IdentityServer4.Services.Interfaces;
using SCSS.Utilities.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;
using static Twilio.Rest.Api.V2010.Account.Call.FeedbackSummaryResource;

namespace SCSS.IdentityServer4.Services.Implementations
{
    public class SMSService : ISMSService
    {
        public async Task SendSMS(string phone, string message)
        {
            var phoneNumber = $"+84{phone.Substring(1)}";
            try
            {
                var result = await MessageResource.CreateAsync(
                    body: message,
                    from: new Twilio.Types.PhoneNumber(AppSettingValues.TwilioPhoneNumber),
                    to: new Twilio.Types.PhoneNumber(phoneNumber)
                );

                // var statusRes = result.Status;
            }
            catch (Exception ex)
            {
                
            }      
        }
    }
}

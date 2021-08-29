using SCSS.IdentityServer4.Services.Interfaces;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Constants;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace SCSS.IdentityServer4.Services.Implementations
{
    public class SMSService : ISMSService
    {
        private readonly ILoggerService _logger;

        public SMSService(ILoggerService logger)
        {
            _logger = logger;
        }

        public async Task SendSMS(string phone, string message)
        {
            try
            {
                var phoneNum = "+84" + phone.Substring(1);
                var result = await MessageResource.CreateAsync(
                     body: message,
                     from: new Twilio.Types.PhoneNumber(AppSettingValues.TwilioPhoneNumber),
                     to: new Twilio.Types.PhoneNumber(phoneNum)
                );
                _logger.LogInfo(LoggerMessages.SendSMSSuccess(message, phone));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, LoggerMessages.SendSMSFail(message, phone));
            }
            
        }
    }
}

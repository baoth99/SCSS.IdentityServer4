using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Configurations
{
    public class AppSettingValues
    {
        public static int CommandTimeout => ConfigurationHelper.GetValue<int>(AppSettingKeys.SystemConfig.CommandTimeOut);

        public static string SqlConnectionString => ConfigurationHelper.GetValue<string>(AppSettingKeys.ConnectionString.SQLConnectionString);

        public static bool ReadScaleOut => ConfigurationHelper.GetValue<bool>(AppSettingKeys.SystemConfig.ReadScaleOut);

        public static string TwilioAccountSID => ConfigurationHelper.GetValue<string>(AppSettingKeys.Twilio.AccountSID);

        public static string TwilioAuthToken => ConfigurationHelper.GetValue<string>(AppSettingKeys.Twilio.AuthToken);

        public static string TwilioPhoneNumber => ConfigurationHelper.GetValue<string>(AppSettingKeys.Twilio.PhoneNumber);

    }
}

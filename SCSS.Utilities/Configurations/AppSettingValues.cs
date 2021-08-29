
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

        public static string AWSRegion => ConfigurationHelper.GetValue<string>(AppSettingKeys.AWS.Region);

        public static string AWSCloudWatchAccessKey => ConfigurationHelper.GetValue<string>(AppSettingKeys.AWS.CloudWatchAccessKey);

        public static string AWSCloudWatchSecrectKey => ConfigurationHelper.GetValue<string>(AppSettingKeys.AWS.CloudWatchSecrectKey);

        public static string AWSCloudWatchLogGroup => ConfigurationHelper.GetValue<string>(AppSettingKeys.AWS.CloudWatchLogGroup);

        public static string LoggingConfig => ConfigurationHelper.GetValue<string>(AppSettingKeys.Logging.Config);

    }
}

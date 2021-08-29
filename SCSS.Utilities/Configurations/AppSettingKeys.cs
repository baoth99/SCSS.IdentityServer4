using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Configurations
{
    public class AppSettingKeys
    {
        public static class ConnectionString
        {
            public const string SQLConnectionString = "ConnectionStrings:SQLConnectionString";
        }

        public static class SystemConfig
        {
            public const string CommandTimeOut = "SystemConfig:CommandTimeout";
            public const string ReadScaleOut = "SystemConfig:ReadScaleOut";
        }

        public static class Twilio
        {
            public const string AccountSID = "Twilio:AccountSID";
            public const string AuthToken = "Twilio:AuthToken";
            public const string PhoneNumber = "Twilio:PhoneNumber";
        }

        public static class AWS
        {
            public const string CloudWatchAccessKey = "AWSService:CloudWatchAccessKey";
            public const string CloudWatchSecrectKey = "AWSService:CloudWatchSecrectKey";
            public const string CloudWatchLogGroup = "AWSService:CloudWatchLogGroup";
            public const string Region = "AWSService:Region";
        }

        public static class Logging
        {
            public const string Config = "Logging:Config";
        }
    }
}

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

        public static class Sercurity
        {
            public const string SercurityId = "Sercurity:SercurityId";
            public const string SercuritySecret = "Sercurity:SercuritySecret";
        }
    }
}

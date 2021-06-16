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
            public const string IndentityServer4SQLConnectionString = "ConnectionStrings:IndentityServer4SQLConnectionString";
            public const string IndentitySQLConnectionString = "ConnectionStrings:IndentitySQLConnectionString";
        }

        public static class SystemConfig
        {
            public const string CommandTimeOut = "SystemConfig:CommandTimeout";
            public const string ReadScaleOut = "SystemConfig:ReadScaleOut";
        }
    }
}

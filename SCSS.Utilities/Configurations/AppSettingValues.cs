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

        public static string IndentityServer4SqlConnectionString => ConfigurationHelper.GetValue<string>(AppSettingKeys.ConnectionString.IndentityServer4SQLConnectionString);

        public static string IndentitySqlConnectionString => ConfigurationHelper.GetValue<string>(AppSettingKeys.ConnectionString.IndentitySQLConnectionString);

        public static bool ReadScaleOut => ConfigurationHelper.GetValue<bool>(AppSettingKeys.SystemConfig.ReadScaleOut);

    }
}

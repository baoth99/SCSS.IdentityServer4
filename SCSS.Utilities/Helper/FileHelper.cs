
namespace SCSS.Utilities.Helper
{
    public class FileHelper
    {
        public static string ContentRootPath { get; set; }

        public static string GetFileConfig(string fileName)
        {
            return $"{ContentRootPath}\\" +
                   $"{fileName}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Helper
{
    public class StringHelper
    {
        public static bool IsBlank(string text)
        {
            return string.IsNullOrEmpty(text);
        }
    }
}

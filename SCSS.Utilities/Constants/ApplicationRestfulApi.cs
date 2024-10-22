﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Constants
{
    public class ApplicationRestfulApi
    {
        public const string BaseApiUrl = "/api/identity/[controller]/";

        public const string ApplicationProduce = "application/json";

        public const string ApplicationContentType = "application/json; charset-utf-8";

        public const string ApplicationConsumes = "application/x-www-form-urlencoded";
    }
}

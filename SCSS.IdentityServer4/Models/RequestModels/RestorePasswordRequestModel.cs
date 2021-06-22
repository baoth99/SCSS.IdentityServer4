using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.IdentityServer4.Models.RequestModels
{
    public class RestorePasswordRequestModel
    {
        public string Phone { get; set; }

        public string OTP { get; set; }

        public string NewPassword { get; set; }
    }
}

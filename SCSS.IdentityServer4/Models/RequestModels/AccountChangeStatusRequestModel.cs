using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.IdentityServer4.Models.RequestModels
{
    public class AccountChangeStatusRequestModel
    {
        public string Id { get; set; }

        public string Status { get; set; }
    }
}

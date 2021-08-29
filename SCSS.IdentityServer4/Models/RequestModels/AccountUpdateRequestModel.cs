using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.IdentityServer4.Models.RequestModels
{
    public class AccountUpdateRequestModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public int Gender { get; set; }

        public string Address { get; set; }

        public string BirthDate { get; set; }

        public string Image { get; set; }

        public string IDCard { get; set; }
    }
}

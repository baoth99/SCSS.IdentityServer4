using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.IdentityServer4.Enities
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<IdentityUserClaim<string>> Claims { get; set; } = new List<IdentityUserClaim<string>>();

        public bool? IsEnabled { get; set; }
    }
}

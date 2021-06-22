using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Validations.ValidationModel
{
    public class AccountRegisterValidationModel
    {
        public string Name { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public bool Gender { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string BirthDate { get; set; }

        public string Image { get; set; }

        public string IDCard { get; set; }
    }
}

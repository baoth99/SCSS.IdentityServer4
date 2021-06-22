using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Helper;
using SCSS.Utilities.Validations.ValidationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Validations
{
    public class AccountValidations
    {
        public static ApiResponseModel UpdateValidation(AccountUpdateValidationModel model)
        {
            var invalidList = new List<string>();

            if (StringHelper.IsBlank(model.Id))
            {
                invalidList.Add(model.Id);
            }
            if (StringHelper.IsBlank(model.Name))
            {
                invalidList.Add(model.Name);
            }
            if (StringHelper.IsBlank(model.UserName))
            {
                invalidList.Add(model.UserName);
            }
            if (StringHelper.IsBlank(model.BirthDate))
            {
                invalidList.Add(model.BirthDate);
            }
            if (StringHelper.IsBlank(model.PhoneNumber))
            {
                invalidList.Add(model.PhoneNumber);
            }

            return null;
        }

        public static ApiResponseModel RegisterValidation(AccountRegisterValidationModel model)
        {
            var invalidList = new List<string>();


            return null;
        }
    }
}

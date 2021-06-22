using AutoMapper;
using SCSS.IdentityServer4.Models.RequestModels;
using SCSS.Utilities.Validations.ValidationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.IdentityServer4.AutoMapper
{
    public class AutoMapperAccount : Profile
    {
        public AutoMapperAccount()
        {
            CreateMap<AccountUpdateRequestModel, AccountUpdateValidationModel>();
            CreateMap<AccountRegistrationRequestModel, AccountRegisterValidationModel>();
        }
    }
}

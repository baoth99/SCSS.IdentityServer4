using SCSS.IdentityServer4.Models.RequestModels;
using SCSS.Utilities.BaseResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.IdentityServer4.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ApiResponseModel> RegisterAccount(AccountRegistrationRequestModel model, string role, int status);

        Task<ApiResponseModel> UpdateAccount(AccountUpdateRequestModel model);

        Task<ApiResponseModel> ChangeStatus(AccountChangeStatusRequestModel model);

        Task<ApiResponseModel> SendOTPToRegister(string phone);

        Task<ApiResponseModel> ConfirmOTPToRegister(string otp, string phone);

        Task<ApiResponseModel> SenOTPToLogin(string phone);

        Task<ApiResponseModel> SendOTPToRestorePassword(string phone);

        Task<ApiResponseModel> ConfirmOTPToRestore(ConfirmOTPRequestModel model);

        Task<ApiResponseModel> RestorePassword(RestorePasswordRequestModel model);

        Task<ApiResponseModel> ChangePassword(ChangePasswordRequestModel model);

        
    }
}

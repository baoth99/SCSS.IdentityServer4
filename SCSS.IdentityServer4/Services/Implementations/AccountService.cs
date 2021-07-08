using AutoMapper;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using SCSS.IdentityServer4.Data.Identity;
using SCSS.IdentityServer4.Models.RequestModels;
using SCSS.IdentityServer4.Services.Interfaces;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Validations;
using SCSS.Utilities.Validations.ValidationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SCSS.IdentityServer4.Services.Implementations
{
    public class AccountService : IAccountService
    {
        #region Services 

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly DataProtectorTokenProvider<ApplicationUser> _dataProtectorTokenProvider;

        private readonly PhoneNumberTokenProvider<ApplicationUser> _phoneNumberTokenProvider;

        private readonly ISMSService _SMSService;

        #endregion

        #region Constructor

        public AccountService(UserManager<ApplicationUser> userManager,
                              DataProtectorTokenProvider<ApplicationUser> dataProtectorTokenProvider,
                              PhoneNumberTokenProvider<ApplicationUser> phoneNumberTokenProvider,
                              ISMSService SMSService)
        {
            _userManager = userManager;
            _dataProtectorTokenProvider = dataProtectorTokenProvider;
            _phoneNumberTokenProvider = phoneNumberTokenProvider;
            _SMSService = SMSService;
        }

        #endregion

        #region Register Account

        /// <summary>
        /// Registers the account.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="role">The role.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public async Task<ApiResponseModel> RegisterAccount(AccountRegistrationRequestModel model, string role, int status)
        {
            var existAccount = await _userManager.FindByNameAsync(model.Phone);

            if (existAccount != null)
            {
                return ApiBaseResponse.Error(MessageCode.PhoneNumberWasExisted);
            }


            var user = new ApplicationUser()
            {
                UserName = model.Phone,
                PhoneNumber = model.Phone,
                PhoneNumberConfirmed = BooleanConstants.TRUE,
                Email = model.Email,
                Status = status,
            };

            #region  Add Identity Claims

            user.Claims.Add(new IdentityUserClaim<string>()
            {
                ClaimType = JwtClaimTypes.Address,
                ClaimValue = model.Address,
            });

            user.Claims.Add(new IdentityUserClaim<string>()
            {
                ClaimType = JwtClaimTypes.PhoneNumber,
                ClaimValue = model.Phone
            });

            user.Claims.Add(new IdentityUserClaim<string>()
            {
                ClaimType = JwtClaimTypes.Role,
                ClaimValue = role
            });

            user.Claims.Add(new IdentityUserClaim<string>()
            {
                ClaimType = JwtClaimTypes.Gender,
                ClaimValue = model.Gender ? "Male" : "Female"
            });

            user.Claims.Add(new IdentityUserClaim<string>()
            {
                ClaimType = JwtClaimTypes.BirthDate,
                ClaimValue = model.BirthDate
            });

            user.Claims.Add(new IdentityUserClaim<string>()
            {
                ClaimType = JwtClaimTypes.Name,
                ClaimValue = model.Name
            });

            user.Claims.Add(new IdentityUserClaim<string>()
            {
                ClaimType = JwtClaimTypes.Email,
                ClaimValue = model.Email
            });

            user.Claims.Add(new IdentityUserClaim<string>()
            {
                ClaimType = JwtClaimTypes.Picture,
                ClaimValue = model.Image
            });

            user.Claims.Add(new IdentityUserClaim<string>()
            {
                ClaimType = JwtClaimTypes.Id,
                ClaimValue = model.IDCard
            });

            #endregion

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var currentUser = await _userManager.FindByNameAsync(model.Phone);
                await _userManager.AddToRoleAsync(currentUser, role);

                var accountId = Guid.Parse(currentUser.Id);

                return ApiBaseResponse.OK(accountId);
            }

            return ApiBaseResponse.Error();
        }

        #endregion

        #region Update Account

        /// <summary>
        /// Updates the account.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<ApiResponseModel> UpdateAccount(AccountUpdateRequestModel model)
        {
            //Validation


            // Get Account
            var account = await _userManager.FindByIdAsync(model.Id);
            // Check Account is existed !
            if (account == null)
            {
                return ApiBaseResponse.NotFound(MessageCode.DataNotFound);
            }
          
            // Get Claim
            var claims = await _userManager.GetClaimsAsync(account);

            var exceptClaims = new List<string>()
            {
                JwtClaimTypes.Role,
                JwtClaimTypes.PhoneNumber,
            };

            // Remove ROlE and PHONENUMBER. We don't update PhoneNumber and Role
            claims.ToList().ForEach(item =>
            {
                if (exceptClaims.Contains(item.Type))
                {
                    claims.Remove(item);
                }
            });

            // Remove Old Claim
            await _userManager.RemoveClaimsAsync(account, claims);

            // Get New Claims
            var updateClaims  = new List<Claim>();

            #region Add Claim

            updateClaims.Add(new Claim(JwtClaimTypes.Address, model.Address));

            updateClaims.Add(new Claim(JwtClaimTypes.Gender, model.Gender ? "Male" : "Female"));

            updateClaims.Add(new Claim(JwtClaimTypes.BirthDate, model.BirthDate));

            updateClaims.Add(new Claim(JwtClaimTypes.Name, model.Name));

            updateClaims.Add(new Claim(JwtClaimTypes.Email, model.Email));

            updateClaims.Add(new Claim(JwtClaimTypes.Picture, model.Image));

            updateClaims.Add(new Claim(JwtClaimTypes.Id, model.IDCard));

            #endregion

            // Add New Claims
            await _userManager.AddClaimsAsync(account, updateClaims);


            return ApiBaseResponse.OK("Update Successful");
        }

        #endregion

        #region Change Status

        /// <summary>
        /// Changes the status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public async Task<ApiResponseModel> ChangeStatus(string id, int status)
        {
            var account = await _userManager.FindByIdAsync(id);

            if (account == null)
            {
                return ApiBaseResponse.NotFound(MessageCode.DataNotFound);
            }

            if (!AccountStatus.StatusCollection.Contains(status))
            {
                return ApiBaseResponse.Error(MessageCode.DataInvalid);
            }

            account.Status = status;

            var result = await _userManager.UpdateAsync(account);
            if (result.Succeeded)
            {
                return ApiBaseResponse.OK();
            }

            return ApiBaseResponse.Error(MessageCode.UpdateFail);
        }

        #endregion

        #region Send OTP

        public async Task<ApiResponseModel> SendOTP(string phone)
        {
            var account = await _userManager.FindByNameAsync(phone);
            if (account == null)
            {
                return ApiBaseResponse.NotFound(MessageCode.DataNotFound);
            }

            var otp = await _phoneNumberTokenProvider.GenerateAsync("verify_number", _userManager, account);

            // Call SMS service here 
            await _SMSService.SendSMS(phone, $"Message From SCSS {otp}");

            return ApiBaseResponse.OK();
        }

        #endregion

        #region Send OTP to Restore Password

        /// <summary>
        /// Sends the otp to restore password.
        /// </summary>
        /// <param name="phone">The phone.</param>
        /// <returns></returns>
        public async Task<ApiResponseModel> SendOTPToRestorePassword(string phone)
        {
            var account = await _userManager.FindByNameAsync(phone);
            if (account == null)
            {
                return ApiBaseResponse.NotFound(MessageCode.DataNotFound);
            }

            var otp = await _userManager.GenerateChangePhoneNumberTokenAsync(account, phone);

            // Call SMS service here 
            var message = SMSMesage.SMSForRestorePasswod(otp);
            await _SMSService.SendSMS(phone, message);

            return ApiBaseResponse.OK();

        }

        #endregion

        #region Restore Password

        /// <summary>
        /// Restores the password.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<ApiResponseModel> RestorePassword(RestorePasswordRequestModel model)
        {
            var account = await _userManager.FindByNameAsync(model.Phone);

            if (account == null)
            {
                return ApiBaseResponse.NotFound(MessageCode.DataNotFound);
            }

            var result = await _userManager.VerifyChangePhoneNumberTokenAsync(account, model.OTP, model.Phone);

            if (!result)
            {
                return ApiBaseResponse.Error(MessageCode.OTPInvalid);
            }

            var validate = new PasswordValidator<ApplicationUser>();
            var res = await validate.ValidateAsync(_userManager, account, model.NewPassword);

            if (!res.Succeeded)
            {
                return ApiBaseResponse.Error(MessageCode.PasswordInValid);
            }


            var token = await _userManager.GeneratePasswordResetTokenAsync(account);

            var resetPassResult = await _userManager.ResetPasswordAsync(account, token, model.NewPassword);

            if (resetPassResult.Succeeded)
            {
                return ApiBaseResponse.OK(MessageCode.ResetPasswordSuccess);
            }

            return ApiBaseResponse.Error(MessageCode.ResetPasswordUnSuccess);
        }

        #endregion

        #region Change Password

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<ApiResponseModel> ChangePassword(ChangePasswordRequestModel model)
        {
            var account = await _userManager.FindByIdAsync(model.Id);

            if (account == null)
            {
                return ApiBaseResponse.NotFound(MessageCode.DataNotFound);
            }

            var validate = new PasswordValidator<ApplicationUser>();
            var res = await validate.ValidateAsync(_userManager, account, model.NewPassword);

            if (!res.Succeeded)
            {
                return ApiBaseResponse.Error(MessageCode.PasswordInValid);
            }

            var result = await _userManager.ChangePasswordAsync(account, model.OldPassword, model.NewPassword);

            if (result.Succeeded)
            {
                return ApiBaseResponse.OK(MessageCode.ChangePasswordSuccess);
            }

            return ApiBaseResponse.Error(MessageCode.ChangePasswordUnSuccess);
        }

        #endregion
    }
}

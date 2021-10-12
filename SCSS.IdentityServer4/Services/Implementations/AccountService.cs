using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using OtpNet;
using SCSS.IdentityServer4.Data.Identity;
using SCSS.IdentityServer4.Models.RequestModels;
using SCSS.IdentityServer4.Services.Interfaces;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SCSS.IdentityServer4.Services.Implementations
{
    public class AccountService : IAccountService
    {
        #region Services 

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly PhoneNumberTokenProvider<ApplicationUser> _phoneNumberTokenProvider;

        private readonly ISMSService _SMSService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IMemoryCache _cache;

        private readonly ILoggerService _logger;

        #endregion

        #region Constructor

        public AccountService(UserManager<ApplicationUser> userManager,
                              PhoneNumberTokenProvider<ApplicationUser> phoneNumberTokenProvider,
                              ISMSService SMSService, 
                              IHttpContextAccessor httpContextAccessor,
                              IMemoryCache cache,
                              ILoggerService logger)
        {
            _userManager = userManager;
            _phoneNumberTokenProvider = phoneNumberTokenProvider;
            _SMSService = SMSService;
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
            _logger = logger;
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
            if (!Regex.IsMatch(model.Phone, RegularExpression.PhoneRegex))
            {
                _logger.LogError(LoggerMessages.PhoneNotMatch(model.Phone, "Phone Number for register"));
                return ApiBaseResponse.Error(MessageCode.PhoneNumberInvalid);
            }

            var key = $"Token-{model.Phone}";

            var registerTokenSession = _cache.Get<string>(key);

            if (registerTokenSession != model.RegisterToken)
            {
                _logger.LogError(LoggerMessages.RegisterTokenError(model.Phone));
                return ApiBaseResponse.Error(MessageCode.InvalidToken);
            }

            _cache.Remove(key);


            var existAccount = await _userManager.FindByNameAsync(model.Phone);

            if (existAccount != null)
            {
                _logger.LogError(LoggerMessages.PhoneIsExisted(model.Phone, $"Register account with {role}"));
                return ApiBaseResponse.Error(MessageCode.PhoneNumberWasExisted);
            }

            var user = new ApplicationUser()
            {
                UserName = model.Phone,
                PhoneNumber = model.Phone,
                PhoneNumberConfirmed = BooleanConstants.TRUE,
                Email = StringHelper.ValidateString(model.Email),
                Status = status,
                LockoutEnabled = BooleanConstants.FALSE,
                EmailConfirmed = BooleanConstants.FALSE
            };

            user.ClientId = role switch
            {
                AccountRoleConstants.SELLER => ClientIdConstant.SellerMobileApp,
                AccountRoleConstants.COLLECTOR => ClientIdConstant.CollectorMobileApp,
                AccountRoleConstants.DEALER => ClientIdConstant.DealerMobileApp,
                AccountRoleConstants.DEALER_MEMBER => ClientIdConstant.DealerMobileApp,
                _ => ClientIdConstant.WebAdmin,
            };

            #region  Add Identity Claims

            user.Claims.Add(new IdentityUserClaim<string>()
            {
                ClaimType = JwtClaimTypes.Address,
                ClaimValue = StringHelper.ValidateString(model.Address)
            });

            user.Claims.Add(new IdentityUserClaim<string>()
            {
                ClaimType = JwtClaimTypes.Role,
                ClaimValue = StringHelper.ValidateString(role)
            });

            user.Claims.Add(new IdentityUserClaim<string>()
            {
                ClaimType = JwtClaimTypes.Gender,
                ClaimValue = StringHelper.ValidateString(model.Gender.ToString())
            });

            user.Claims.Add(new IdentityUserClaim<string>()
            {
                ClaimType = JwtClaimTypes.BirthDate,
                ClaimValue = StringHelper.ValidateString(model.BirthDate)
            });

            user.Claims.Add(new IdentityUserClaim<string>()
            {
                ClaimType = JwtClaimTypes.Name,
                ClaimValue = StringHelper.ValidateString(model.Name)
            });

            user.Claims.Add(new IdentityUserClaim<string>()
            {
                ClaimType = JwtClaimTypes.Picture,
                ClaimValue = StringHelper.ValidateString(model.Image)
            });

            user.Claims.Add(new IdentityUserClaim<string>()
            {
                ClaimType = JwtClaimTypes.Id,
                ClaimValue = StringHelper.ValidateString(model.IDCard)
            });

            #endregion

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var currentUser = await _userManager.FindByNameAsync(model.Phone);
                await _userManager.AddToRoleAsync(currentUser, role);
                _logger.LogInfo(LoggerMessages.RegisterUserSucess(model.Phone, model.Name, role));

                return ApiBaseResponse.OK(currentUser.Id);
            }

            _logger.LogInfo(LoggerMessages.RegisterUserFail(model.Phone, model.Name, role));
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
            // Get Account
            var account = await _userManager.FindByIdAsync(model.Id);
            // Check Account is existed !
            if (account == null)
            {
                _logger.LogError(LoggerMessages.UserNotFound(model.Id));
                return ApiBaseResponse.NotFound(MessageCode.DataNotFound);
            }

          
            account.Email = StringHelper.ValidateString(model.Email);
            await _userManager.UpdateAsync(account);
            

            // Get Now Claim
            var claims = await _userManager.GetClaimsAsync(account);

            // Remove ROlE. We don't update Role
            claims.ToList().ForEach(item =>
            {
                if (item.Type == JwtClaimTypes.Role)
                {
                    claims.Remove(item);
                }
            });

            // Remove Old Claim
            await _userManager.RemoveClaimsAsync(account, claims);

            // Get New Claims
            var updateClaims  = new List<Claim>();

            #region Add Claim

            updateClaims.Add(new Claim(JwtClaimTypes.Address, StringHelper.ValidateString(model.Address)));

            updateClaims.Add(new Claim(JwtClaimTypes.Gender, StringHelper.ValidateString(model.Gender.ToString())));

            updateClaims.Add(new Claim(JwtClaimTypes.BirthDate, StringHelper.ValidateString(model.BirthDate)));

            updateClaims.Add(new Claim(JwtClaimTypes.Name, StringHelper.ValidateString(model.Name)));

            updateClaims.Add(new Claim(JwtClaimTypes.Picture, StringHelper.ValidateString(model.Image)));

            updateClaims.Add(new Claim(JwtClaimTypes.Id, StringHelper.ValidateString(model.IDCard)));

            #endregion

            // Add New Claims
            await _userManager.AddClaimsAsync(account, updateClaims);
            _logger.LogInfo(LoggerMessages.UpdateUserSuccess(model.Id));

            return ApiBaseResponse.OK("Update Successful");
        }

        #endregion

        #region Send OTP To Login

        /// <summary>
        /// Sens the otp to login.
        /// </summary>
        /// <param name="phone">The phone.</param>
        /// <returns></returns>
        public async Task<ApiResponseModel> SenOTPToLogin(string phone)
        {
            var account = await _userManager.FindByNameAsync(phone);

            if (account == null)
            {
                _logger.LogError(LoggerMessages.UserNotFound(phone));
                return ApiBaseResponse.NotFound(MessageCode.DataNotFound);
            }
            if (account.ClientId != IdentityServer.ClientId)
            {
                _logger.LogError(LoggerMessages.ClientIdInvalid);
                return ApiBaseResponse.NotFound(MessageCode.ClientIdInvalid);
            }
            if (account.Status == AccountStatus.BANNING)
            {
                _logger.LogError(LoggerMessages.AccountStatus("blocked"));
                return ApiBaseResponse.NotFound(MessageCode.AccountIsBanning);
            }
            if (account.Status == AccountStatus.NOT_APPROVED)
            {
                _logger.LogError(LoggerMessages.AccountStatus("not approved"));
                return ApiBaseResponse.NotFound(MessageCode.AccountIsNotApproved);
            }

            var otp = await _phoneNumberTokenProvider.GenerateAsync(IdentityServer.VerifyOTP, _userManager, account);

            // Send SMS
            var message = SMSMesage.SMSForLogin(otp);

            await _SMSService.SendSMS(phone, message);


            return ApiBaseResponse.OK();
        }

        #endregion

        #region Change Status

        /// <summary>
        /// Changes the status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public async Task<ApiResponseModel> ChangeStatus(AccountChangeStatusRequestModel model)
        {
            var account = await _userManager.FindByIdAsync(model.Id);

            if (account == null)
            {
                _logger.LogError(LoggerMessages.UserNotFound(model.Id));
                return ApiBaseResponse.NotFound(MessageCode.DataNotFound);
            }

            _ = int.TryParse(model.Status, out int statusInt);

            if (!AccountStatus.StatusCollection.Contains(statusInt))
            {
                _logger.LogError(LoggerMessages.AccStatusInvalid);
                return ApiBaseResponse.Error(MessageCode.DataInvalid);
            }

            account.Status = statusInt;

            var result = await _userManager.UpdateAsync(account);
            if (result.Succeeded)
            {
                _logger.LogInfo(LoggerMessages.ChangeStatus(account.Id, model.Status));
                return ApiBaseResponse.OK();
            }

            return ApiBaseResponse.Error(MessageCode.UpdateFail);
        }

        #endregion

        #region Send OTP to Register

        /// <summary>
        /// Send OTP To Register
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public async Task<ApiResponseModel> SendOTPToRegister(string phone)
        {
            if (!Regex.IsMatch(phone, RegularExpression.PhoneRegex))
            {
                _logger.LogError(LoggerMessages.PhoneNotMatch(phone, "Phone Number for register"));
                return ApiBaseResponse.Error(MessageCode.PhoneNumberInvalid);
            }

            var existAccount = await _userManager.FindByNameAsync(phone);

            if (existAccount != null)
            {
                _logger.LogError(LoggerMessages.PhoneIsExisted(phone, $"Send OTP to register"));
                return ApiBaseResponse.Error(MessageCode.PhoneNumberWasExisted);
            }

            var secretKey = Guid.NewGuid().ToByteArray();

            var totp = new Totp(secretKey, mode: OtpHashMode.Sha512);

            var otp = totp.ComputeTotp(DateTime.UtcNow);

            _httpContextAccessor.HttpContext.Session.SetString(phone, otp);

            // Send SMS here !!!!
            var message = SMSMesage.OTP(otp);
            await _SMSService.SendSMS(phone, message);

            return ApiBaseResponse.OK();
        }

        #endregion

        #region Confirm OTP to Register

        /// <summary>
        /// Confirms the otp to register.
        /// </summary>
        /// <param name="otp">The otp.</param>
        /// <param name="phone">The phone.</param>
        /// <returns></returns>
        public async Task<ApiResponseModel> ConfirmOTPToRegister(string otp, string phone)
        {
            if (!Regex.IsMatch(phone, RegularExpression.PhoneRegex))
            {
                _logger.LogError(LoggerMessages.PhoneNotMatch(phone, "Confirm phone Number to register"));
                return ApiBaseResponse.Error(MessageCode.PhoneNumberInvalid);
            }

            var otpSession = _httpContextAccessor.HttpContext.Session.GetString(phone);

            if (otpSession == otp)
            {
                string token = Convert.ToBase64String(Encoding.ASCII.GetBytes(otp + phone + DateTime.Now));
                _httpContextAccessor.HttpContext.Session.Remove(phone);

                var key = $"Token-{phone}";

                _cache.Set<string>(key, token, TimeSpan.FromMinutes(3));
                _logger.LogInfo(LoggerMessages.OTPConfirm("Register"));
                return ApiBaseResponse.OK(token);
            }
            _logger.LogError(LoggerMessages.OTPInvalid("Register"));
            return ApiBaseResponse.Error(MessageCode.OTPInvalid);
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
            var message = SMSMesage.OTP(otp);
            await _SMSService.SendSMS(phone, message);

            _logger.LogInfo(LoggerMessages.OTPConfirm("Restore password"));

            return ApiBaseResponse.OK();

        }

        #endregion

        #region Confirm OTP to RestorePassword

        /// <summary>
        /// Confirms the otp to restore.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<ApiResponseModel> ConfirmOTPToRestore(ConfirmOTPRequestModel model)
        {
            var account = await _userManager.FindByNameAsync(model.Phone);

            if (account == null)
            {
                return ApiBaseResponse.NotFound(MessageCode.DataNotFound);
            }

            var result = await _userManager.VerifyChangePhoneNumberTokenAsync(account, model.OTP, model.Phone);

            if (!result)
            {
                _logger.LogInfo(LoggerMessages.OTPInvalid("Restore Password"));
                return ApiBaseResponse.Error(MessageCode.OTPInvalid);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(account);

            return ApiBaseResponse.OK(token);

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
                _logger.LogError(LoggerMessages.UserNotFound(model.Phone));
                return ApiBaseResponse.NotFound(MessageCode.DataNotFound);
            }

            var resetPassResult = await _userManager.ResetPasswordAsync(account, model.Token, model.NewPassword);

            if (resetPassResult.Succeeded)
            {
                _logger.LogError(LoggerMessages.RestorePasswordSuccess(account.Id));
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

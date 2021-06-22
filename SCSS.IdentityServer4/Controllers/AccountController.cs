using AutoMapper;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SCSS.IdentityServer4.AuthenFilter;
using SCSS.IdentityServer4.Constants;
using SCSS.IdentityServer4.Data.Identity;
using SCSS.IdentityServer4.Models.RequestModels;
using SCSS.IdentityServer4.Services.Interfaces;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Validations;
using SCSS.Utilities.Validations.ValidationModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.IdentityServer4.Controllers
{
    public class AccountController : BaseController
    {
        #region Services

        /// <summary>
        /// The user manager
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// The account service
        /// </summary>
        private readonly IAccountService _accountService;

        #endregion

        #region Constructor

        public AccountController(UserManager<ApplicationUser> userManager, IAccountService accountService)
        {
            _userManager = userManager;
            _accountService = accountService;
        }

        #endregion

        #region Register Account Demo

        [HttpPost]
        [Route(AccountUrlDefinition.RegisterAccountDemo)]
        public async Task<IActionResult> RegisterAccountDemo([FromForm] AccountRegistrationRequestModelDemo model)
        {
            var user = new ApplicationUser()
            {
                UserName = model.Phone,
                PhoneNumber = model.Phone,
            };

            #region Add Identity Claims

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
                ClaimValue = model.Role
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
                var currentUser = await _userManager.FindByNameAsync(model.UserName);
                await _userManager.AddToRoleAsync(currentUser, model.Role);
                return new JsonResult(new { message = "create successful" });
            }
            else
            {
                return new JsonResult(new { message = "create Fail" });
            }
        }

        #endregion

        #region Register Dealer Account

        /// <summary>
        /// Registers the dealer account.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route(AccountUrlDefinition.RegisterDealerAccount)]
        [Consumes(ApplicationRestfulApi.ApplicationConsumes)]
        [ServiceFilter(typeof(AuthenFilterAttribute))]
        public async Task<ApiResponseModel> RegisterDealerAccount([FromForm] AccountRegistrationRequestModel model)
        {
            var result = await _accountService.RegisterAccount(model, AccountRoleConstants.DEALER, AccountStatus.NOT_APPROVED);
            return result;
        }

        #endregion

        #region Register Collector Account

        /// <summary>
        /// Registers the collector account.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route(AccountUrlDefinition.RegisterCollectorAccount)]
        [ServiceFilter(typeof(AuthenFilterAttribute))]
        public async Task<ApiResponseModel> RegisterCollectorAccount([FromForm] AccountRegistrationRequestModel model)
        {
            var result = await _accountService.RegisterAccount(model, AccountRoleConstants.DEALER, AccountStatus.NOT_APPROVED);
            return result;
        }

        #endregion

        #region Register Seller Account

        /// <summary>
        /// Registers the seller account.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route(AccountUrlDefinition.RegisterSellerAccount)]
        [ServiceFilter(typeof(AuthenFilterAttribute))]
        public async Task<ApiResponseModel> RegisterSellerAccount([FromForm] AccountRegistrationRequestModel model)
        {
            var result = await _accountService.RegisterAccount(model, AccountRoleConstants.SELLER, AccountStatus.ACTIVE);
            return result;
        }

        #endregion

        #region Update Account

        /// <summary>
        /// Updates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        [Route(AccountUrlDefinition.UpdateAccount)]
        [ServiceFilter(typeof(AuthenFilterAttribute))]
        public async Task<ApiResponseModel> Update([FromForm] AccountUpdateRequestModel model)
        {
            var result = await _accountService.UpdateAccount(model);
            return result;
        }

        #endregion

        #region Change Account Status

        /// <summary>
        /// Changes the status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        [HttpPut]
        [Route(AccountUrlDefinition.ChangeStatus)]
        [ServiceFilter(typeof(AuthenFilterAttribute))]
        public async Task<ApiResponseModel> ChangeStatus([FromForm] string id, int status)
        {
            var result = await _accountService.ChangeStatus(id, status);
            return result;
        }

        #endregion

        #region Restore Password

        /// <summary>
        /// Restores the password.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        /// [HttpPut]
        [HttpPost]
        [Route(AccountUrlDefinition.RestorePassword)]
        [ServiceFilter(typeof(AuthenFilterAttribute))]
        public async Task<ApiResponseModel> RestorePassword([FromForm] RestorePasswordRequestModel model)
        {
            var result = await _accountService.RestorePassword(model.Phone, model.OTP, model.NewPassword);
            return result;
        }

        #endregion

        #region Send OTP

        /// <summary>
        /// Sends the otp.
        /// </summary>
        /// <param name="phone">The phone.</param>
        /// <returns></returns>
        [HttpPost]
        [Route(AccountUrlDefinition.SendOTP)]
        [ServiceFilter(typeof(AuthenFilterAttribute))]
        public async Task<ApiResponseModel> SendOTP([FromForm] string phone)
        {
            var result = await _accountService.SendOTP(phone);
            return result;
        }

        #endregion

        #region Send OTP to Restore Password

        /// <summary>
        /// Sends the otp to restore password.
        /// </summary>
        /// <param name="phone">The phone.</param>
        /// <returns></returns>
        [HttpPost]
        [Route(AccountUrlDefinition.SendOTPToRestorePassword)]
        [ServiceFilter(typeof(AuthenFilterAttribute))]
        public async Task<ApiResponseModel> SendOTPToRestorePassword([FromForm] string phone)
        {
            var result = await _accountService.SendOTPToRestorePassword(phone);
            return result;
        }

        #endregion

        #region Change Password

        [HttpPost]
        [Route(AccountUrlDefinition.ChangePassword)]
        [ServiceFilter(typeof(AuthenFilterAttribute))]
        public async Task<ApiResponseModel> ChangePassword([FromForm] ChangePasswordRequestModel model)
        {
            var result = await _accountService.ChangePassword(model.Id, model.OldPassword, model.NewPassword);
            return result;
        }
        #endregion

    }
}

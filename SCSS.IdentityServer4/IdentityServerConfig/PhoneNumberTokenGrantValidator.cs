using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using SCSS.IdentityServer4.Data.Identity;
using SCSS.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.IdentityServer4.IdentityServerConfig
{
    public class PhoneNumberTokenGrantValidator : IExtensionGrantValidator
    {
        private readonly PhoneNumberTokenProvider<ApplicationUser> _phoneNumberTokenProvider;
        private readonly UserManager<ApplicationUser> _userManager;


        public PhoneNumberTokenGrantValidator(PhoneNumberTokenProvider<ApplicationUser> phoneNumberTokenProvider,
                                              UserManager<ApplicationUser> userManager)
        {
            _phoneNumberTokenProvider = phoneNumberTokenProvider;
            _userManager = userManager;
        }

        public string GrantType => IdentityServer.GrantType;

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var raw = context.Request.Raw;

            var credential = raw.Get(OidcConstants.TokenRequest.GrantType);
            if (credential != GrantType)
            {
                return;
            }

            var phoneNumber = raw.Get("phone_number");
            var verificationToken = raw.Get("verification_token");
            var clientId = raw.Get(OidcConstants.TokenRequest.ClientId);

            var user = _userManager.Users.SingleOrDefault(x => x.PhoneNumber == _userManager.NormalizeName(phoneNumber));


            if (user != null)
            {
                if (user.ClientId != clientId)
                {
                    var errorCode = new Dictionary<string, object>();
                    errorCode.Add(CommonsConstants.MessageCode, MessageCode.ClientIsWrong);
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient, "ClientId is invalid");
                    context.Result.CustomResponse = errorCode;
                    return;
                }
                var result = await _phoneNumberTokenProvider.ValidateAsync(IdentityServer.VerifyOTP, verificationToken, _userManager, user);

                if (!result)
                {
                    var errorCode = new Dictionary<string, object>();
                    errorCode.Add(CommonsConstants.MessageCode, MessageCode.OTPInvalid);
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "Invalid token or verification id");
                    context.Result.CustomResponse = errorCode;
                    return;
                }
                context.Result = new GrantValidationResult(user.Id, OidcConstants.AuthenticationMethods.ConfirmationBySms);
            }
            else
            {
                var errorCode = new Dictionary<string, object>();
                errorCode.Add(CommonsConstants.MessageCode, MessageCode.PhoneNumberInvalid);
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "Phone number is invalid");
                context.Result.CustomResponse = errorCode;
                return;
            }
        }
    }
}

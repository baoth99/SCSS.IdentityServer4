using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using SCSS.IdentityServer4.Data.Identity;
using SCSS.Utilities.Constants;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCSS.IdentityServer4.IdentityServerConfig
{
    public class CustomTokenResponseGenerator : ICustomTokenRequestValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomTokenResponseGenerator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(CustomTokenRequestValidationContext context)
        {
            var raw = context.Result.ValidatedRequest.Raw;

            var grantType = raw.Get(OidcConstants.TokenRequest.GrantType);
            if (grantType.Equals(OidcConstants.TokenRequest.RefreshToken))
            {
                return;
            }

            if (grantType.Equals(OidcConstants.TokenRequest.Password))
            {
                var username = raw.Get(OidcConstants.TokenRequest.UserName);
               
                var account = await _userManager.FindByNameAsync(username);


                var clientId = raw.Get(OidcConstants.TokenRequest.ClientId);

                if (!account.ClientId.Equals(clientId))
                {
                    context.Result.IsError = BooleanConstants.TRUE;
                    var errorCode = new Dictionary<string, object>();
                    errorCode.Add(CommonsConstants.MessageCode, MessageCode.ClientIsWrong);
                    context.Result.Error = $"Account don't belong to {clientId} client";
                    context.Result.CustomResponse = errorCode;
                    return;
                }

                if (!account.Status.Equals(AccountStatus.ACTIVE))
                {
                    context.Result.IsError = BooleanConstants.TRUE;
                    var errorCode = new Dictionary<string, object>();
                    switch (account.Status)
                    {
                        case AccountStatus.NOT_APPROVED:
                            errorCode.Add(CommonsConstants.MessageCode, MessageCode.AccountIsNotApproved);
                            context.Result.Error = "Account is not approved !";                           
                            break;
                        case AccountStatus.BANNING:
                            errorCode.Add(CommonsConstants.MessageCode, MessageCode.AccountIsBanning);
                            context.Result.Error = "Account is banning !";
                            break;
                        default:
                            errorCode.Add(CommonsConstants.MessageCode, MessageCode.AccountIsInvalid);
                            context.Result.Error = "Account is invalid !";
                            break;
                    }

                    context.Result.CustomResponse = errorCode;
                }
            }
        }
    }
}

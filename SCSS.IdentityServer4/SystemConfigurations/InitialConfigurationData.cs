using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.IdentityServer4.SystemConfigurations
{
    public static class InitialConfigurationData
    {
        public static IEnumerable<ApiScope> ApiScopes =>
                               new List<ApiScope>
                               {
                                    new ApiScope()
                                    {
                                        Name = "SCSS.WebApi",
                                        DisplayName = "SCSS WebApi",
                                        UserClaims =
                                        {
                                            "given_name", "family_name", "role"
                                        },


                                    }
                               };

        public static IEnumerable<ApiResource> ApiResources =>
                                new List<ApiResource>
                                {
                                    new ApiResource()
                                    {
                                        Name = "SCSS.WebApi",
                                        DisplayName = "SCSS WebApi",
                                        ApiSecrets = new List<Secret>
                                        {
                                            new Secret("82e25d5b-40a7-41a0-9a71-4f6766ff7fb6".Sha256())
                                        },
                                        Scopes= { "SCSS.WebApi",
                                            IdentityServerConstants.StandardScopes.OpenId,
                                            IdentityServerConstants.StandardScopes.Profile,
                                            "roles"
                                        },
                                        UserClaims =
                                        {
                                            "given_name", "family_name", "role",
                                        },

                                    }
                                };


        public static IEnumerable<IdentityResource> IdentityResources =>
                                new List<IdentityResource>
                                {
                                    new IdentityResources.OpenId(),
                                    new IdentityResources.Profile(),
                                    new IdentityResources.Email(),
                                    new IdentityResources.Phone(),
                                    new IdentityResource
                                    {
                                        Name = "roles",
                                        DisplayName = "Roles",
                                        UserClaims = { JwtClaimTypes.Role }
                                    }
                                };

        // Define Client will connect to Identity Server
        public static IEnumerable<Client> Clients =>
                                new List<Client>
                                {
                                    new Client
                                    {
                                        ClientId = "WebAdmin-FrontEnd",
                                        ClientName = "WebAdmin-Application",                                     
                                        // no interactive user, use the clientid/secret for authentication phone_number_token
                                        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                                        // secret for authentication using SHA256
                                        ClientSecrets =
                                        {
                                            new Secret("ad12f5f7-a6b5-42d1-8771-0a09338ca8ff".Sha256())
                                        },
                                        AlwaysSendClientClaims = true,
                                        AlwaysIncludeUserClaimsInIdToken = true,
                                        RefreshTokenExpiration = TokenExpiration.Sliding,//when refreshing the token,
                                                                               //the lifetime of the refresh token will be renewed
                                                                               //when refreshing the token, the lifetime of the refresh token will be renewed
                                        SlidingRefreshTokenLifetime = 3600 * 2,
                                        AccessTokenType = AccessTokenType.Jwt, 
                                        UpdateAccessTokenClaimsOnRefresh = true,
                                        // scopes that client can access to
                                        AllowedScopes = { "SCSS.WebApi",
                                            IdentityServerConstants.StandardScopes.OpenId,
                                            IdentityServerConstants.StandardScopes.Profile,
                                            IdentityServerConstants.StandardScopes.OfflineAccess,
                                            "role"
                                            },
                                        AccessTokenLifetime = 3600 * 2 , //Set Token lifetime
                                        RefreshTokenUsage = TokenUsage.OneTimeOnly,
                                        AllowOfflineAccess = true,
                                        RedirectUris = { "https://localhost:5001/signin-oidc" }

                                    },
                                    new Client
                                    {
                                        ClientId = "Seller-Mobile",
                                        ClientName = "Seller Mobile Application",                                     
                                        // no interactive user, use the clientid/secret for authentication phone_number_token
                                        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                                        // secret for authentication using SHA256
                                        ClientSecrets =
                                        {
                                            new Secret("0dba7c0e-cc7f-475c-89e6-c3c4c7e093a6".Sha256())
                                        },
                                        AlwaysSendClientClaims = true,
                                        AlwaysIncludeUserClaimsInIdToken = true,
                                        RefreshTokenExpiration = TokenExpiration.Sliding,//when refreshing the token,
                                                                               //the lifetime of the refresh token will be renewed
                                                                               //when refreshing the token, the lifetime of the refresh token will be renewed
                                        SlidingRefreshTokenLifetime = 3600 * 2,
                                        AccessTokenType = AccessTokenType.Jwt,
                                        UpdateAccessTokenClaimsOnRefresh = true,
                                        // scopes that client can access to
                                        AllowedScopes = { "SCSS.WebApi",
                                            IdentityServerConstants.StandardScopes.OpenId,
                                            IdentityServerConstants.StandardScopes.Profile,
                                            IdentityServerConstants.StandardScopes.OfflineAccess,
                                            "role"
                                            },
                                        AccessTokenLifetime = 3600 * 2 , //Set Token lifetime
                                        RefreshTokenUsage = TokenUsage.OneTimeOnly,
                                        AllowOfflineAccess = true,

                                    }
                                };
    }
}

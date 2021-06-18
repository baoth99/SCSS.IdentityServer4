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
                                        Name = "SCSS.WebAdmin.Scope",
                                        DisplayName = "SCSS Web Admin Scope",
                                        UserClaims =
                                        {
                                            JwtClaimTypes.Address,
                                            JwtClaimTypes.PhoneNumber,
                                            JwtClaimTypes.Role,
                                            JwtClaimTypes.Gender,
                                            JwtClaimTypes.BirthDate,
                                            JwtClaimTypes.Name,
                                            JwtClaimTypes.Email,
                                            JwtClaimTypes.Picture,
                                            JwtClaimTypes.Id
                                        },
                                    },
                                    new ApiScope()
                                    {
                                        Name = "SCSS.SellerMobileApp.Scope",
                                        DisplayName = "SCSS Seller Mobile App Scope",
                                        UserClaims =
                                        {
                                            JwtClaimTypes.Address,
                                            JwtClaimTypes.PhoneNumber,
                                            JwtClaimTypes.Role,
                                            JwtClaimTypes.Gender,
                                            JwtClaimTypes.BirthDate,
                                            JwtClaimTypes.Name,
                                            JwtClaimTypes.Email,
                                            JwtClaimTypes.Picture,
                                            JwtClaimTypes.Id
                                        },
                                    },
                                    new ApiScope()
                                    {
                                        Name = "SCSS.CollectorMobileApp.Scope",
                                        DisplayName = "SCSS Collector Mobile App Scope",
                                        UserClaims =
                                        {
                                            JwtClaimTypes.Address,
                                            JwtClaimTypes.PhoneNumber,
                                            JwtClaimTypes.Role,
                                            JwtClaimTypes.Gender,
                                            JwtClaimTypes.BirthDate,
                                            JwtClaimTypes.Name,
                                            JwtClaimTypes.Email,
                                            JwtClaimTypes.Picture,
                                            JwtClaimTypes.Id
                                        },
                                    },
                                    new ApiScope()
                                    {
                                        Name = "SCSS.DealerMobileApp.Scope",
                                        DisplayName = "SCSS Dealer Mobile App Scope",
                                        UserClaims =
                                        {
                                            JwtClaimTypes.Address,
                                            JwtClaimTypes.PhoneNumber,
                                            JwtClaimTypes.Role,
                                            JwtClaimTypes.Gender,
                                            JwtClaimTypes.BirthDate,
                                            JwtClaimTypes.Name,
                                            JwtClaimTypes.Email,
                                            JwtClaimTypes.Picture,
                                            JwtClaimTypes.Id
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
                                        Scopes= {
                                            "SCSS.WebAdmin.Scope",
                                            "SCSS.SellerMobileApp.Scope",
                                            "SCSS.CollectorMobileApp.Scope",
                                            "SCSS.DealerMobileApp.Scope",
                                            IdentityServerConstants.StandardScopes.OpenId,
                                            IdentityServerConstants.StandardScopes.Profile,
                                            IdentityServerConstants.StandardScopes.OfflineAccess,
                                            "role"
                                        },
                                        UserClaims =
                                        {
                                            JwtClaimTypes.Address,
                                            JwtClaimTypes.PhoneNumber,
                                            JwtClaimTypes.Role,
                                            JwtClaimTypes.Gender,
                                            JwtClaimTypes.BirthDate,
                                            JwtClaimTypes.Name,
                                            JwtClaimTypes.Email,
                                            JwtClaimTypes.Picture,
                                            JwtClaimTypes.Id
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
                                        Name = "role",
                                        DisplayName = "Role",
                                        UserClaims = { JwtClaimTypes.Role }
                                    },
                                    new IdentityResource
                                    {
                                        Name = "id_card",
                                        DisplayName = "IDCard",
                                        UserClaims = { JwtClaimTypes.Id }
                                    }
                                };

        // Define Client will connect to Identity Server
        public static IEnumerable<Client> Clients =>
                                new List<Client>
                                {
                                    new Client
                                    {
                                        ClientId = "SCSS-WebAdmin-FrontEnd",
                                        ClientName = "Scrap WebAdmin Application",                                     
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
                                        AllowedScopes = { 
                                            "SCSS.WebAdmin.Scope",
                                            IdentityServerConstants.StandardScopes.OpenId,
                                            IdentityServerConstants.StandardScopes.Profile,
                                            IdentityServerConstants.StandardScopes.Phone,
                                            IdentityServerConstants.StandardScopes.Email,
                                            IdentityServerConstants.StandardScopes.OfflineAccess,
                                            "role",
                                            "id_card"
                                            },
                                        AllowAccessTokensViaBrowser = true,
                                        AccessTokenLifetime = 3600 * 4 , //Set Token lifetime
                                        RefreshTokenUsage = TokenUsage.OneTimeOnly,
                                        AllowOfflineAccess = true,

                                    },
                                    new Client
                                    {
                                        ClientId = "SCSS-Seller-Mobile",
                                        ClientName = "Scrap Seller Mobile Application",                                     
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
                                        SlidingRefreshTokenLifetime = 3600 * 4,
                                        AccessTokenType = AccessTokenType.Jwt,
                                        UpdateAccessTokenClaimsOnRefresh = true,
                                        // scopes that client can access to
                                        AllowedScopes = { 
                                            "SCSS.SellerMobileApp.Scope",
                                            IdentityServerConstants.StandardScopes.OpenId,
                                            IdentityServerConstants.StandardScopes.Profile,
                                            IdentityServerConstants.StandardScopes.Phone,
                                            IdentityServerConstants.StandardScopes.Email,
                                            IdentityServerConstants.StandardScopes.OfflineAccess,
                                            "role",
                                            "id_card"
                                            },
                                        AccessTokenLifetime = 3600 * 2 , //Set Token lifetime
                                        RefreshTokenUsage = TokenUsage.OneTimeOnly,
                                        AllowOfflineAccess = true,

                                    },
                                    new Client
                                    {
                                        ClientId = "SCSS-Collector-Mobile",
                                        ClientName = "Scrap Collector Mobile Application",                                     
                                        // no interactive user, use the clientid/secret for authentication phone_number_token
                                        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                                        // secret for authentication using SHA256
                                        ClientSecrets =
                                        {
                                            new Secret("8adb9943-07b1-4ada-91dd-ba91560085e5".Sha256())
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
                                        AllowedScopes = {
                                            "SCSS.CollectorMobileApp.Scope",
                                            IdentityServerConstants.StandardScopes.OpenId,
                                            IdentityServerConstants.StandardScopes.Profile,
                                            IdentityServerConstants.StandardScopes.Phone,
                                            IdentityServerConstants.StandardScopes.Email,
                                            IdentityServerConstants.StandardScopes.OfflineAccess,
                                            "role",
                                            "id_card"
                                            },
                                        AccessTokenLifetime = 3600 * 4 , //Set Token lifetime
                                        RefreshTokenUsage = TokenUsage.OneTimeOnly,
                                        AllowOfflineAccess = true,
                                    },
                                    new Client
                                    {
                                        ClientId = "SCSS-Dealer-Mobile",
                                        ClientName = "Scrap Dealer Mobile Application",                                     
                                        // no interactive user, use the clientid/secret for authentication phone_number_token
                                        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                                        // secret for authentication using SHA256
                                        ClientSecrets =
                                        {
                                            new Secret("bf2388cd-907f-4933-8f4c-1239c0674437".Sha256())
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
                                        AllowedScopes = {
                                            "SCSS.DealerMobileApp.Scope",
                                            IdentityServerConstants.StandardScopes.OpenId,
                                            IdentityServerConstants.StandardScopes.Profile,
                                            IdentityServerConstants.StandardScopes.Phone,
                                            IdentityServerConstants.StandardScopes.Email,
                                            IdentityServerConstants.StandardScopes.OfflineAccess,
                                            "role",
                                            "id_card"
                                            },
                                        AccessTokenLifetime = 3600 * 4 , //Set Token lifetime
                                        RefreshTokenUsage = TokenUsage.OneTimeOnly,
                                        AllowOfflineAccess = true,
                                    }
                                };
    }
}

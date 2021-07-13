using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SCSS.IdentityServer4.Data.Identity;

namespace SCSS.IdentityServer4.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly IIdentityServerInteractionService _interaction;

        public LogoutModel(SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger, IIdentityServerInteractionService interaction)
        {
            _signInManager = signInManager;
            _logger = logger;
            _interaction = interaction;
        }

        public void OnGet()
        {
            //await OnPost();
        }

        //public async Task<IActionResult> OnGet(string returnUrl)
        //{
        //    await _signInManager.SignOutAsync();
        //    _logger.LogInformation("User logged out.");
        //    var logoutId = Request.Query["logoutId"];

        //    var data = Request.RouteValues;

        //    if (returnUrl != null)
        //    {
        //        return LocalRedirect(returnUrl);
        //    }
        //    else if (!string.IsNullOrEmpty(logoutId))
        //    {
        //        var logoutContext = await _interaction.GetLogoutContextAsync(logoutId);
        //        returnUrl = logoutContext.PostLogoutRedirectUri;

        //        if (!string.IsNullOrEmpty(returnUrl))
        //        {
        //            return Redirect(returnUrl);
        //        }
        //        else
        //        {
        //            return Page();
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToPage();
        //    }
        //}

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            var logoutId = Request.Query["logoutId"];

            var data = Request.RouteValues;

            if (returnUrl == null)
            {
                return Redirect("https://localhost:44344/Identity/Account/Login?ReturnUrl=%2Fconnect%2Fauthorize%2Fcallback%3Fclient_id%3DSCSS-WebAdmin-FrontEnd%26redirect_uri%3Dhttp%253A%252F%252Flocalhost%253A4200%252Fsignin-oidc%26response_type%3Did_token%2520token%26scope%3DSCSS.WebAdmin.Scope%2520profile%2520openid%2520offline_access%2520role%2520phone%2520id_card%2520email%26state%3Db1e7f146b8234d4295f2eca779285d88%26nonce%3D55e59041d3c745ba86445fb3bb6660f2");
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}

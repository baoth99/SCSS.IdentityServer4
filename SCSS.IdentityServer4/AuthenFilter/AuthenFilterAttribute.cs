using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using SCSS.IdentityServer4.SystemExtensions;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;

namespace SCSS.IdentityServer4.AuthenFilter
{
    public class AuthenFilterAttribute : ActionFilterAttribute
    {

        private readonly IClientStore _clientStore;

        public AuthenFilterAttribute(IClientStore clientStore)
        {
            _clientStore = clientStore;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Request.Headers.TryGetValue("client_id", out StringValues clientIdVal);

            var clientId = clientIdVal.ToString();

            var result = _clientStore.FindClientByIdAsync(clientId).Result;
            if (result == null)
            {
                context.ActionFilterResult(MessageCode.ClientIdInvalid ,"client_id is invalid", HttpStatusCodes.Unauthorized);
            }

            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                if (context.Result is ObjectResult objectResult)
                {
                    if (objectResult.Value is ApiResponseModel result && !result.IsSuccess)
                    {
                        switch (result.StatusCode)
                        {
                            case HttpStatusCodes.Unauthorized:
                                context.Result = new UnauthorizedObjectResult("Permission denied, wrong credentials or user not be allowed access.");
                                break;
                            case HttpStatusCodes.NotFound:
                                context.Result = new NotFoundObjectResult("The Record not found.");
                                break;
                            case HttpStatusCodes.Forbidden:
                                context.Result = new StatusCodeResult(HttpStatusCodes.Forbidden);
                                break;
                            case HttpStatusCodes.BadRequest:
                                context.Result = new ObjectResult("Something wrong");
                                break;
                        }
                    }
                }
            }
            catch
            {
                // Ignore
            }
            base.OnActionExecuted(context);
        }
    }
}

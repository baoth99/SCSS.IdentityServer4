using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.IdentityServer4.AuthenFilter
{
    public class AuthenFilterAttribute : ActionFilterAttribute
    {

        public AuthenFilterAttribute()
        {

        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            
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

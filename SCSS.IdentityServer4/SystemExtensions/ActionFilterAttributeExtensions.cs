using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SCSS.Utilities.BaseResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.IdentityServer4.SystemExtensions
{
    internal static class ActionFilterAttributeExtensions
    {
        public static void ActionFilterResult(this ActionExecutingContext context,string messageCode, string message, int httpStatusCode)
        {
            context.Result = new JsonResult(new ErrorResponseModel()
            {
                StatusCode = httpStatusCode,
                MessageCode = messageCode,
                Message = message
            })
            { StatusCode = httpStatusCode };
        }
    }
}

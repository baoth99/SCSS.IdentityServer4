using Microsoft.AspNetCore.Mvc;
using SCSS.Utilities.Constants;


namespace SCSS.IdentityServer4.Controllers
{
    [ApiController]
    [Route(ApplicationRestfulApi.BaseApiUrl)]
    [Produces(ApplicationRestfulApi.ApplicationProduce)]
    [Consumes(ApplicationRestfulApi.ApplicationConsumes)]
    public class BaseController : ControllerBase
    {
    }
}

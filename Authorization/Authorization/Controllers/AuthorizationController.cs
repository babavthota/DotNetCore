using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authorization.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Controllers
{
    [Route("api/[controller]/[action]")]
    [CustomAuthorize("ApplicationUser","Read,Create")]
    public class AuthorizationController : ControllerBase
    {       

    }
}

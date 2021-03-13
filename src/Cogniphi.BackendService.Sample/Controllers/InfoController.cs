using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cogniphi.Platform.Middleware.Authorization.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cogniphi.BackendService.Sample.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class InfoController : ControllerBase
    {
        [HttpGet]
        [Authorize(AuthPolices.VerbBasedPolicy)]
        public ActionResult<string> Get()
        {
            return "sample backend";
        }
    }
}
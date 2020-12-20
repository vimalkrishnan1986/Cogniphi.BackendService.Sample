using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Steeltoe.Discovery;
using Steeltoe.Common.Discovery;

namespace Cogniphi.BackendService.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        DiscoveryHttpClientHandler _handler;
        private readonly ILogger _logger;

        public ValuesController(ILogger<ValuesController> logger, IDiscoveryClient client)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _handler = new DiscoveryHttpClientHandler(client);
        }
        [HttpGet]
        public ActionResult<string> Get()
        {
            _logger.LogInformation("Get call");
            return "value";
        }
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }
    }
}

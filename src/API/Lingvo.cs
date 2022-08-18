using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace API
{
    public class Lingvo
    {
        private readonly VortaroContext _context;
        public Lingvo(VortaroContext context)
        {
            _context = context;
        }

        [FunctionName("Lingvo")]
        public Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            return Task.FromResult<IActionResult>(new OkObjectResult(_context.Lingvoj.ToList()));
        }
    }
}

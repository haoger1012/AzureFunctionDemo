using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureFunctionDemo.Service;

namespace AzureFunctionDemo
{
    public class DITest
    {
        private readonly IMyService myService;

        public DITest(IMyService myService)
        {
            this.myService = myService;
        }

        [FunctionName("DITest")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            int result = myService.Add(1, 2);

            return new OkObjectResult(result);
        }
    }
}

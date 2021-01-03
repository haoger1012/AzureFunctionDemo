using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureFunctionDemo.Model;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureFunctionDemo
{
    public static class OrderFunction
    {
        [FunctionName("GetAllOrders")]
        public static async Task<IActionResult> GetAllOrders(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "order")] HttpRequest req,
            [Table("order")] CloudTable orderTable,
            ILogger log)
        {
            var query = new TableQuery<Order>();
            var segment = await orderTable.ExecuteQuerySegmentedAsync(query, null);
            var result = segment.Select(x => new OrderDto
            {
                OrderId = x.RowKey,
                CustomerEmail = x.CustomerEmail,
                CustomerName = x.CustomerName
            });

            return new OkObjectResult(result);
        }

        [FunctionName("GetOrder")]
        public static IActionResult GetOrder(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "order/{id}")] HttpRequest req,
            string id,
            [Table("order", "Http", "{id}")] Order order,
            ILogger log)
        {
            if (order == null)
            {
                return new NotFoundResult();
            }

            var result = new OrderDto
            {
                OrderId = order.RowKey,
                CustomerEmail = order.CustomerEmail,
                CustomerName = order.CustomerName
            };

            return new OkObjectResult(result);
        }

        [FunctionName("CreateOrder")]
        [return: Table("order")]
        public static Order AddOrder(
             [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "order")] OrderDto dto,
             [Queue("queue-demo")] out OrderDto queues,
             ILogger log)
        {
            var order = new Order
            {
                PartitionKey = "Http",
                RowKey = Guid.NewGuid().ToString(),
                CustomerEmail = dto.CustomerEmail,
                CustomerName = dto.CustomerName
            };

            queues = new OrderDto
            {
                OrderId = order.RowKey,
                CustomerEmail = order.CustomerEmail,
                CustomerName = order.CustomerName
            };

            return order;
        }

        //[FunctionName("CreateOrder")]
        //public static IActionResult AddOrder(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "order")] OrderDto dto,
        //    [Table("order")] out Order order,
        //    ILogger log)
        //{
        //    order = new Order
        //    {
        //        PartitionKey = "Http",
        //        RowKey = Guid.NewGuid().ToString(),
        //        CustomerEmail = dto.CustomerEmail,
        //        CustomerName = dto.CustomerName
        //    };

        //    return new OkResult();
        //}
    }
}

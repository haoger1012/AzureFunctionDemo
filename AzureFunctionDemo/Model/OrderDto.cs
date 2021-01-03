using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AzureFunctionDemo.Model
{
    public class OrderDto
    {
        [ReadOnly(true)]
        public string OrderId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
    }
}

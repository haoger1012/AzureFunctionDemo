using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctionDemo.Model
{
    public class Order : TableEntity
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string Product { get; set; }
    }
}

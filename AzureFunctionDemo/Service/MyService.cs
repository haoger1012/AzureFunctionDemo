using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctionDemo.Service
{
    public class MyService : IMyService
    {
        public int Add(int a, int b)
        {
            return a + b;
        }
    }
}

using RetailLib.Attributes;
using System;
using RetailTypes;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace RetailLib
{
    public abstract class Operator
    {
        public static string Token;
        internal static HttpClient http;
        public Operator()
        {
            http = new HttpClient();
        }
        public static Operator Search() 
        {
            var operators = Assembly.GetExecutingAssembly().GetTypes().Where(i=>i.IsSubclassOf(typeof(Operator)));
            var ofd = operators.FirstOrDefault(o => o.GetCustomAttribute<OperatorAttribute>().Vatin == Driver.FiscalOperator);
            return (Operator)Activator.CreateInstance(ofd);
        }
        public abstract Task<List<Receipt>> PullReceipts(DateTime date);
    }
}

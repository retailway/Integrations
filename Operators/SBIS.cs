using RetailLib.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace RetailLib.Operators
{
    [Operator("7605015260")]
    public class SBIS : Operator
    {
        public override Task<List<Receipt>> PullReceipts(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}

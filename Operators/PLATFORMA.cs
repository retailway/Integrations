using RetailLib.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailLib.Operators
{
    [Operator("9715260691")]
    public class PLATFORMA : Operator
    {
        public override Task<List<Receipt>> PullReceipts(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}

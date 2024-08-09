using RetailWay.Integrations.Attributes;
using System;
using RetailWay.Types;
using System.Collections.Generic;
using RetailWay.Integrations.Raw;
using System.Threading.Tasks;
using RetailWay.Types.Elements;
using RetailWay.Types.Enums;

namespace RetailWay.Integrations.Operators
{
    [Operator("9715260691")]
    public class PLATFORMA : Operator
    {
        public PLATFORMA() : base() { }
        public override async Task<List<Receipt>> PullReceipts(DateTime date)
        {
            var raw = await PlatformOfd.GetReceipts(Token, date, RegId: Driver.RegNumber);
            var res = new List<Receipt>();
            if (raw.Length == 0) return null;
            foreach (var obj in raw)
            {
                var corr = obj.receiptCode == 31;
                var retu = obj.operationType % 2 == 0;
                var sell = obj.operationType / 3 == 0;
                var origin = new Receipt
                {
                    Id = obj.fiscalDocumentNumber,
                    Cashier = obj.cashier,
                    FiscalSign = obj.fiscalSign,
                    StorageId = obj.fiscalDriveNumber,
                    Payment = new Payment(obj.cashTotalSum, obj.ecashTotalSum, obj.prepaidSum, obj.provisionSum, obj.creditSum),
                    Positions = new Position[obj.items.Length],
                    Date = obj.receiptDate,
                    Operation = (corr, retu, sell).ToOperation()
                };
                for (var i = 0; i < obj.items.Length; i++)
                {
                    var item = obj.items[i];
                    origin.Positions[i] = new Position
                    {
                        Name = item.name,
                        Price = item.price,
                        Quantity = (decimal)item.quantity,
                        Calculation = (CalculationMethod)item.paymentType,
                        Type = (SubjectType)item.productType,
                        Tax = (TaxType)new int[7] { -1, 1, 0, 3, 2, 4, 5 }[item.nds_1199],
                        // todo fix
                        //MeasureUnit = (MeasureUnit)item.mea
                    };
                }
                res.Add(origin);
            }
            return res;
        }
    }
}

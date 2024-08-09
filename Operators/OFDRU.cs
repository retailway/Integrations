using RetailWay.Integrations.Attributes;
using RetailWay.Types;
using RetailWay.Types.Enums;
using RetailWay.Types.Elements;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RetailWay.Integrations.Raw;

namespace RetailWay.Integrations.Operators
{
    [Operator("7841465198")]
    public class OFDRU : Operator
    {
        public OFDRU() : base() { }
        public override async Task<List<Receipt>> PullReceipts(DateTime date)
        {
            var raw = await OfdRu.GetInfoReceipts(Driver.CompanyVatin, Token, Driver.RegNumber, date);
            var res = new List<Receipt>();
            if (raw.Data.Length == 0) return null;
            foreach (var obj in raw.Data)
            {
                var corr = obj.IsCorrection;
                var retu = obj.OperationType.StartsWith("Return");
                var sell = obj.OperationType.EndsWith("ncome");
                var origin = new Receipt
                {
                    Id = obj.DocNumber,
                    Cashier = obj.Operator,
                    FiscalSign = obj.DecimalFiscalSign,
                    StorageId = obj.FnNumber,
                    Payment = new Payment(obj.CashSumm, obj.ECashSumm, obj.PrepaidSumm, obj.ProvisionSumm, obj.CreditSumm),
                    Positions = new Position[obj.Depth],
                    Date = obj.DocDateTime,
                    Operation = (corr, retu, sell).ToOperation()
                };
                for (var i = 0; i < obj.Depth; i++)
                {
                    var item = obj.Items[i];
                    origin.Positions[i] = new Position
                    {
                        Name = item.Name,
                        Price = item.Price,
                        Quantity = (decimal)item.Quantity,
                        Calculation = (CalculationMethod)item.CalculationMethod,
                        Type = (SubjectType)item.SubjectType,
                        Tax = (TaxType)item.NDS_Rate,
                        MeasureUnit = (MeasureUnit)item.ProductUnitOfMeasure
                    };
                }
                res.Add(origin);
            }
            return res;
        }
        
    }
}
 
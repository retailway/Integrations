using Newtonsoft.Json.Linq;
using RetailWay.Integrations.Attributes;
using RetailWay.Types;
using RetailWay.Types.Enums;
using RetailWay.Types.Elements;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Globalization;

namespace RetailWay.Integrations.Operators
{
    [Operator("7841465198")]
    public class OFDRU : Operator
    {
        public OFDRU() : base() { }
        public override async Task<List<Receipt>> PullReceipts(DateTime date)
        {
            var res = new List<Receipt>();
            var url = "https://ofd.ru/api/integration/v2/inn/{0}/kkt/{1}/receipts-info?dateFrom={2}&dateTo={3}&AuthToken={4}";
            url = string.Format(url, Driver.CompanyVatin, Driver.RegNumber, date.ToString("yyyy-MM-dd"), date.ToString("yyyy-MM-ddTHH:mm:ss"), Token);
            var req = new HttpRequestMessage(HttpMethod.Get, url);
            var resp = await http.SendAsync(req);
            var raw = await resp.Content.ReadAsStringAsync();
            var body = JObject.Parse(raw);
            if (!(body["Data"] is JArray receipts)) return null;
            foreach (var obj in receipts)
            {
                var corr = (bool)obj["IsCorrection"];
                var retu = ((string)obj["OperationType"]).StartsWith("Return");
                var sell = ((string)obj["OperationType"]).EndsWith("ncome");
                var origin = new Receipt
                {
                    Id = (int)obj["DocNumber"],
                    Cashier = (string)obj["Operator"],
                    FiscalSign = (string)obj["FiscalSign"],
                    StorageId = (string)obj["FnNumber"],
                    Payment = new Payment((int)obj["CashSumm"], (int)obj["ECashSumm"]),
                    Positions = new Position[(int)obj["Depth"]],
                    Date = DateTime.ParseExact((string)obj["DocDateTime"], "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                    Operation = (corr, retu, sell).ToOperation()
                };
                for (var i = 0; i < (int)obj["Depth"]; i++)
                {
                    var item = obj["Items"][i];
                    origin.Positions[i] = new Position
                    {
                        Name = (string)item["Name"],
                        Price = (int)item["Price"],
                        Quantity = (decimal)item["Quantity"],
                        Calculation = (CalculationMethod)(int)item["CalculationMethod"],
                        Type = (SubjectType)(int)item["SubjectType"],
                        Tax = (TaxType)(int)item["NDS_Rate"],
                        MeasureUnit = (MeasureUnit)(int)item["ProductUnitOfMeasure"]
                    };
                }
                res.Add(origin);
            }
            return res;
        }
    }
}

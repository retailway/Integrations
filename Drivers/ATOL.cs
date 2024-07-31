using Atol.Drivers10.Fptr;
using RetailTypes;
using RetailTypes.Enums;
using RetailTypes.Elements;
using System.Linq;

namespace RetailLib.Drivers
{
    public class ATOL : Driver
    {
        #region Приватные переменные
        private Fptr driver { get; }
        #endregion
        #region Наследственные переменные
        public override bool IsConnected => driver != null && driver.isOpened();
        #endregion
        #region Конструктор
        public ATOL()
        {
            driver = new Fptr();
        }
        #endregion
        #region Наследственные методы
        public override void Dispose()
        {
            if(driver.isOpened()) 
                driver.close();
            driver.destroy();
        }

        public override void EndSession()
        {
            driver.setParam(Constants.LIBFPTR_PARAM_REPORT_ELECTRONICALLY, IsElectronically);
            driver.setParam(Constants.LIBFPTR_PARAM_REPORT_TYPE, Constants.LIBFPTR_RT_CLOSE_SHIFT);
            driver.report();
        }

        public override void PullInfo()
        {
            if (driver.isOpened())
            {
                driver.setParam(Constants.LIBFPTR_PARAM_DATA_TYPE, Constants.LIBFPTR_DT_CACHE_REQUISITES);
                driver.queryData();
                TotalStorage = driver.getParamString(Constants.LIBFPTR_PARAM_FN_SERIAL_NUMBER);
                RegNumber = driver.getParamString(Constants.LIBFPTR_PARAM_ECR_REGISTRATION_NUMBER);
                FiscalOperator = driver.getParamString(Constants.LIBFPTR_PARAM_OFD_VATIN);

                driver.setParam(Constants.LIBFPTR_PARAM_FN_DATA_TYPE, Constants.LIBFPTR_FNDT_TAG_VALUE);
                driver.setParam(Constants.LIBFPTR_PARAM_TAG_NUMBER, 1018);
                driver.fnQueryData();
                CompanyVatin = driver.getParamString(Constants.LIBFPTR_PARAM_TAG_VALUE);
            }
        }

        public override void StartSession()
        {
            driver.setParam(Constants.LIBFPTR_PARAM_REPORT_ELECTRONICALLY, IsElectronically);
            driver.openShift();
        }

        public override void OpenReceipt(Receipt receipt)
        {
            driver.setParam(Constants.LIBFPTR_PARAM_REPORT_ELECTRONICALLY, IsElectronically);
            driver.setParam(Constants.LIBFPTR_PARAM_RECEIPT_TYPE, ConvertOperation(receipt.Operation));
            driver.openReceipt();
        }

        public override void AddPosition(Position pos)
        {
            driver.setParam(Constants.LIBFPTR_PARAM_COMMODITY_NAME, pos.Name);
            driver.setParam(Constants.LIBFPTR_PARAM_PRICE, pos.Price);
            driver.setParam(Constants.LIBFPTR_PARAM_QUANTITY, (double)pos.Quantity);
            driver.setParam(Constants.LIBFPTR_PARAM_TAX_TYPE, ConvertTax(pos.Tax));
            driver.setParam(1212, (int)pos.Type);
            driver.setParam(1214, (int)pos.Calculation);
            driver.setParam(2108, (int)pos.MeasureUnit);
            SetCodes(pos.Codes);
            driver.registration();
        }

        public override void CloseReceipt(Receipt receipt)
        {
            if (receipt.DoRoundTotal) RoundSum(receipt.Positions);
            if (receipt.Payment.CashSum != 0) 
                Payment(Constants.LIBFPTR_PT_CASH, receipt.Payment.CashSum);
            if (receipt.Payment.EcashSum != 0) 
                Payment(Constants.LIBFPTR_PT_ELECTRONICALLY, receipt.Payment.EcashSum);
            if (driver.closeReceipt() < 0) CancelReceipt();
        }
        public override void Payment(int type, int sum)
        {
            driver.setParam(Constants.LIBFPTR_PARAM_PAYMENT_TYPE, type);
            driver.setParam(Constants.LIBFPTR_PARAM_PAYMENT_SUM, sum);
            driver.payment();
        }
        public override void CancelReceipt() =>
            driver.cancelReceipt();

        public override void Auth(string cashier, string vatin = "000000000000", string passwd = null)
        {
            driver.setParam(1021, cashier);
            driver.setParam(1203, vatin);
            driver.operatorLogin();
        }

        public override void RoundSum(Position[] positions)
        {
            driver.setParam(Constants.LIBFPTR_PARAM_SUM, (positions.Sum(p => p.Total) / 100) * 100);
            driver.receiptTotal();
        }
        public override void SetCodes(Codes codes)
        {
            if (codes.Unknown != "") driver.setParam(1300, codes.Unknown);
            if (codes.EAN8 != "") driver.setParam(1301, codes.EAN8);
            if (codes.EAN13 != "") driver.setParam(1302, codes.EAN13);
            if (codes.ITF14 != "") driver.setParam(1303, codes.ITF14);
            if (codes.GS1 != "") driver.setParam(1304, codes.GS1);
            if (codes.MI != "") driver.setParam(1307, codes.MI);
            if (codes.EGAIS2 != "") driver.setParam(1308, codes.EGAIS2);
            if (codes.EGAIS3 != "") driver.setParam(1309, codes.EGAIS3);
            if (codes.F1 != "") driver.setParam(1320, codes.F1);
            if (codes.F2 != "") driver.setParam(1321, codes.F2);
            if (codes.F3 != "") driver.setParam(1322, codes.F3);
            if (codes.F4 != "") driver.setParam(1323, codes.F4);
            if (codes.F5 != "") driver.setParam(1324, codes.F5);
            if (codes.F6 != "") driver.setParam(1325, codes.F6);
        }
        #endregion
        #region Приватные методы
        private int ConvertTax(TaxType tax) =>
            (new int[] { 2, 7, 4, 8, 5, 6 })[(int)tax];
        private int ConvertOperation(OperationType operation) =>
            (new int[] { 4, 1, 5, 2, 9, 7, 10, 8 })[(int)operation];

        #endregion
    }
}

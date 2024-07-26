using Atol.Drivers10.Fptr;

namespace RetailLib.Drivers
{
    public class ATOL : Driver
    {
        private Fptr driver { get; }

        public ATOL()
        {
            driver = new Fptr();
        }

        public override void Dispose()
        {
            if(driver.isOpened()) 
                driver.close();
            driver.destroy();
        }

        public override void EndSession()
        {
            // todo Реализовать
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
            driver.openShift();
        }
    }
}

using RetailLib.Entities;
using RetailLib.Enums;
using System;

namespace RetailLib
{
    public struct Receipt
    {
        public int Id;
        public string StorageId;
        public string Cashier;
        public string FiscalSign;
        public DateTime Date;
        public OperationType Operation;
        public Position[] Positions;
        public Payment Payment;
    }
}

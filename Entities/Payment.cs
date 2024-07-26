namespace RetailLib.Entities
{
    public struct Payment
    {
        public int CashSum;
        public int EcashSum;
        public Payment (int cash, int ecash)
        {
            CashSum = cash;
            EcashSum = ecash;
        }
        public static bool operator ==(Payment p1, Payment p2) =>
            p1.EcashSum == p2.EcashSum && p1.CashSum == p2.CashSum;
        public static bool operator !=(Payment p1, Payment p2) => !(p1 == p2);
        public static bool operator ==((int c, int e) p1, Payment p2) =>
            p1.e == p2.EcashSum && p1.c == p2.CashSum;
        public static bool operator !=((int c, int e) p1, Payment p2) => !(p1 == p2);
        public static bool operator ==(Payment p1, (int c, int e) p2) =>
            p1.EcashSum == p2.e && p1.CashSum == p2.c;
        public static bool operator !=(Payment p1, (int c, int e) p2) => !(p1 == p2);
    }
}

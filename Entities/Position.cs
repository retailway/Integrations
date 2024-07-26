using RetailLib.Enums;

namespace RetailLib.Entities
{
    public struct Position
    {
        public string Name;
        public decimal Quantity;
        public MeasureUnit MeasureUnit;
        public CalculationMethod Calculation;
        public TaxType Tax;
        public SubjectType Type;
        public int Price;
    }
}

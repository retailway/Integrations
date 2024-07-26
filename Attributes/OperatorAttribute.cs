using System;

namespace RetailLib.Attributes
{
    public class OperatorAttribute: Attribute
    {
        public string Vatin;
        public OperatorAttribute(string vatin)
        {
            Vatin = vatin;
        }
    }
}

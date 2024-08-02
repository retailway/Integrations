using System;

namespace RetailWay.Integrations.Attributes
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

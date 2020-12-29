using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Domain
{
    public class Currency : ValueObject
    {
        public string CurrencyCode { get; set; }
        public bool InUse { get; set; }
        public int DecimalPlaces { get; set; }

        public static Currency None = new Currency { InUse = false };

        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }
    }
}

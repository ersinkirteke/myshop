using Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API
{
    public class FakeCurrencyLookup : ICurrencyLookup
    {
        private static readonly IEnumerable<Currency> currencies = new[]
        {
            new Currency {
                CurrencyCode="TL",
                DecimalPlaces=2,
                InUse=true
            },
            new Currency {
                CurrencyCode="USD",
                DecimalPlaces=2,
                InUse=true
            },
             new Currency {
                CurrencyCode="EUR",
                DecimalPlaces=0,
                InUse=true
            }
        };

        public Currency FindCurrency(string currencyCode) => currencies.FirstOrDefault(x => x.CurrencyCode == currencyCode) ?? Currency.None;
    }
}

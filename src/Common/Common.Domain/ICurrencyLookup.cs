using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Domain
{
    public interface ICurrencyLookup
    {
        Currency FindCurrency(string currencyCode);
    }
}

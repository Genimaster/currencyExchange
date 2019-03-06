using System;

namespace currencyExchange.Models
{
    class CurrencyList
    {
        public string Name { get; set; }

        public string Rate { get; set; }

        public bool Base { get; set; }

        public DateTime Date { get; set; }
    }
}

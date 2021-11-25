using CurrencyConverter.Data;
using CurrencyConverter.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyConverter.Services.Implementation
{
    public class CurrencyConverter : ICurrencyConverter
    {
        public Graph CurrencyGraph { get; set; } = new Graph();
        public void ClearConfiguration()
        {
            CurrencyGraph = new Graph();
        }

        public double Convert(string fromCurrency, string toCurrency, double amount)
        {
            var path = CurrencyGraph.FindPath(fromCurrency, toCurrency).ToArray();

            if (path == null)
                throw new ApplicationException("Can not convert");

            double conversionRate = 1;
            for (int i = 0; path.Length - 1 > i; i++)
            {
                Node from = path[i];
                Node to = path[i + 1];
                conversionRate *= from.Relations[to.Id].ConvertionRate;
            }

            return conversionRate * amount;
        }

        public void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates)
        {
            foreach (var cRate in conversionRates)
                CurrencyGraph.AddEdge(cRate.Item1, cRate.Item2, cRate.Item3);
        }
    }
}

using CurrencyConverter.Data;
using CurrencyConverter.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CurrencyConverter.Services.Implementation
{
    public class CurrencyConverter : ICurrencyConverter
    {
        static CurrencyConverter _instance;
        private CurrencyConverter()
        {

        }

        public static CurrencyConverter GetInstance()
        {
            if (_instance == null)
                _instance = new CurrencyConverter();

            return _instance;
        }

        ReaderWriterLockSlim locker = new ReaderWriterLockSlim();
        public Graph CurrencyGraph { get; set; } = new Graph();
        public void ClearConfiguration()
        {
            locker.EnterWriteLock();
            try
            {
                CurrencyGraph = new Graph();
            }
            finally
            {
                locker.ExitWriteLock();
            }            
        }

        public double Convert(string fromCurrency, string toCurrency, double amount)
        {
            locker.EnterReadLock();
            try
            {
                var path = CurrencyGraph.FindPath(fromCurrency, toCurrency);

                if (path == null)
                    return 0; //throw new ApplicationException("Can not convert");

                double conversionRate = 1;
                for (int i = 0; path.Count - 1 > i; i++)
                {
                    Node from = path[i];
                    Node to = path[i + 1];
                    conversionRate *= from.Relations[to.Id].ConvertionRate;
                }
                return conversionRate * amount;
            }
            finally
            {
                locker.ExitReadLock();
            }            
        }

        public void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates)
        {
            locker.EnterWriteLock();
            try
            {
                foreach (var cRate in conversionRates)
                    CurrencyGraph.AddEdge(cRate.Item1, cRate.Item2, cRate.Item3);
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }
    }
}

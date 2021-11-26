using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CurrencyConverterTest
{
    public class ConcurrentConversionTests
    {
        [Fact]
        public void ConcurrentRun()
        {
            CurrencyConverter.Services.Abstraction.ICurrencyConverter service
               = CurrencyConverter.Services.Implementation.CurrencyConverter.GetInstance();
            service.ClearConfiguration();
            service.UpdateConfiguration(new List<Tuple<string, string, double>>
            {
               new ("CAD", "JPY", 3.1),
               new ( "USD", "CAD",2.2),
               new ( "USD", "GBP",1.5),
               new ( "CAD", "IRP",1),
               new ( "GBP", "NRC",1),
               new ( "GBP", "EUR",1.2),
               new ( "NRC", "CHF",1),
               new ( "NRC", "AFN",1),
               new ( "NRC", "EUR",1),
               new ( "CHF", "AFN",1),
               new ( "AFN", "EUR",1),
               new ( "JPY", "EUR",0.8)
            });

            List<Task<double>> tasks = new List<Task<double>>();
            for (int i = 0; i < 100; i++)
            {
                tasks.Add(new Task<double>(() =>
                {
                    string source = "GBP", dest = "JPY";
                    var res = service.Convert(source, dest, 10);
                    return res;
                }));

            }
            tasks.ForEach(r => r.Start());

            Task.WaitAll(tasks.ToArray());

            Assert.True(tasks.All(r => r.Result == 15d));

        }

        [Fact]
        public void ConcurrentRunWithModification()
        {
            CurrencyConverter.Services.Abstraction.ICurrencyConverter service
               = CurrencyConverter.Services.Implementation.CurrencyConverter.GetInstance();
            service.ClearConfiguration();
            service.UpdateConfiguration(new List<Tuple<string, string, double>>
            {
               new ("CAD", "JPY", 1),
               new ( "USD", "CAD",2),
               new ( "USD", "GBP",4),
               new ( "CAD", "IRP",4),
               new ( "GBP", "NRC",1),
               new ( "GBP", "EUR",5),
               new ( "NRC", "CHF",1),
               new ( "NRC", "AFN",1),
               new ( "NRC", "EUR",1),
               new ( "CHF", "AFN",1),
               new ( "AFN", "EUR",1)
            });

            var rand = new Random();
            List<Task<double>> tasks = new List<Task<double>>();
            for (int i = 0; i < 100; i++)
            {
                tasks.Add(new Task<double>(() =>
                {
                    Task.Delay(rand.Next(1, 1000));
                    string source = "GBP", dest = "JPY";
                    var res = service.Convert(source, dest, 10);
                    return res;
                }));

            }

            tasks.Insert(50, new Task<double>(() =>
            {
                service.ClearConfiguration();
                service.UpdateConfiguration(new List<Tuple<string, string, double>>
                {
                   new ("CAD", "JPY", 1),
                   new ( "USD", "CAD",2),
                   new ( "USD", "GBP",2), //changed
                   new ( "CAD", "IRP",4),
                   new ( "GBP", "NRC",1),
                   new ( "GBP", "EUR",5),
                   new ( "NRC", "CHF",1),
                   new ( "NRC", "AFN",1),
                   new ( "NRC", "EUR",1),
                   new ( "CHF", "AFN",1),
                   new ( "AFN", "EUR",1)
                });

                return 15d;
            }));
            tasks.ForEach(r => r.Start());
            Task.WaitAll(tasks.ToArray());
            var results = tasks.Select(r => r.Result).ToList();

            var acceptedResults = new List<double> {
                15,//changeConfigDummyResult
                0,//between clear and set config result, cant find a path
                10,//secondConfigResult
                5 //firstConfig result
            };
            Assert.True(results.All(r => acceptedResults.Contains(r)));

        }
    }
}

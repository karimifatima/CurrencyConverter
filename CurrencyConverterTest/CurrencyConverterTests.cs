using System;
using System.Collections.Generic;
using Xunit;

namespace CurrencyConverterTest
{
    public class CurrencyConverterTests
    {
        [Fact]
        public void Test1()
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

            string source = "GBP", dest = "JPY";
            var res = service.Convert(source, dest, 10);
            Assert.Equal(15d,res);
        }

        [Fact]
        public void Test2()
        {
            CurrencyConverter.Services.Abstraction.ICurrencyConverter service
                =  CurrencyConverter.Services.Implementation.CurrencyConverter.GetInstance();
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
               new ( "AFN", "EUR",1)
            });

            string source = "GBP", dest = "JPY";
            var res = service.Convert(source, dest, 10);
            Assert.Equal(45.46666666666667d, res);
        }
    }
}

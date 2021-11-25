using CurrencyConverter.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyConverter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();

            var service = new Services.Implementation.CurrencyConverter();
            service.UpdateConfiguration(new List<Tuple<string,string,double>>
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
            var res=service.Convert(source, dest,10);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

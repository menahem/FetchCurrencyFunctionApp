using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(FetchCurrencyFunctionApp.Startup))]

namespace FetchCurrencyFunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IFetchCurrencyFunction, FetchCurrencyFunction>();
            builder.Services.AddSingleton<ExchangeRateService>();
        }
    }
}

using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;

namespace FetchCurrencyFunctionApp
{
    public class FetchCurrencyFunction : IFetchCurrencyFunction
    {
        private readonly ExchangeRateService _exchangeRateService;

        public FetchCurrencyFunction(ExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        [FunctionName("currency")]
        public async Task<string> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "currency/{yearMonth}")] HttpRequest req,
            string yearMonth)
        {
            int year = 2000 + int.Parse(yearMonth.Substring(0, 2));
            int month = int.Parse(yearMonth.Substring(2, 2));

            // Call the ExchangeRateService to retrieve the rates
            return await _exchangeRateService.GetExchangeRates(year, month);

        }
    }
}

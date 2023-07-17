using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FetchCurrencyFunctionApp
{
    public class ExchangeRateService
    {
        private readonly HttpClient _httpClient;
        private Dictionary<string,string> _cache = new Dictionary<string,string>();

        public ExchangeRateService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetExchangeRates(int year, int month)
        {
            string result;
            if (TryGetDataFromCache(year + "" + month, out result))
            {
                return result;
            }
            
            // Construct the API URL using the specified year and month
            string url = $"https://api.freecurrencyapi.com/v1/historical?apikey=YtAKuhegS1UqUIogAd2ODNw8Mf5BFCxdl54PwM46&currencies=EUR" + ConvertToQueryString(year, month);
            
            // Make the HTTP request to the API
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            // Handle the response
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(responseBody);
            var innerJsonObject = jsonObject["data"];
            jsonObject.Remove("data");
            jsonObject.Add("GRAPH", innerJsonObject);
            result = JsonConvert.SerializeObject(jsonObject);

            _cache.Add(year + "" + month, result);
            return result;
        }

        private bool TryGetDataFromCache(string date, out string result)
        {
            return _cache.TryGetValue(date, out result);
        }

        public string ConvertToQueryString(int year, int month)
        {
            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            string fromDate = firstDayOfMonth.ToString("yyyy-MM-dd");
            string toDate = lastDayOfMonth.ToString("yyyy-MM-dd");
            return $"&date_from={fromDate}&date_to={toDate}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using CoinClient.Services;
using System.Threading.Tasks;
using CoinClient.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Xamarin.Forms;

//[assembly: Dependency(typeof(CoinService))]
namespace CoinClient.Services
{
    public class CoinService : ICoinService
    {
        private const string Url = "https://functionapp20171005111758.azurewebsites.net/api/CoinTrendGetter?code=97yZEaSXaBMSf/ENJOQW7FQeqArV/bkPgvqyQlsSFW6yf4GCdJALyA==";

        public async Task<CoinTrend> GetTrend()
        {
            var client = new HttpClient();
            var json = await client.GetStringAsync(Url);
            var trend = JsonConvert.DeserializeObject<CoinTrend>(json);
            return trend;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using CoinClient.Services;
using CoinClient.Models;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(DesignCoinService))]
namespace CoinClient.Services
{
    public class DesignCoinService : ICoinService
    {
        const int OriginalTrend = 1;
        const double OriginalValue = 345.6;
        CoinTrend trend;
        Random random;

        public DesignCoinService()
        {
            random = new Random();
        }

        public Task<CoinTrend> GetTrend() {

            var tcs = new TaskCompletionSource<CoinTrend>(TaskCreationOptions.AttachedToParent);
            if (trend == null)
            {
                trend = new CoinTrend
                {
                    CurrentValue = OriginalValue,
                    Trend = OriginalTrend,
                    Time = DateTime.UtcNow
                };
            }
            else
            {
                trend.CurrentValue += random.Next(10, 500);
                trend.Trend =
                    trend.Trend == 0 ? 1
                    : trend.Trend == 1 ? -1
                    : 0;
            }
            tcs.SetResult(trend);
            return tcs.Task;
        }
    }
}

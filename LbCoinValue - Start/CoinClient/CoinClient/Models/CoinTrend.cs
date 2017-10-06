using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoinClient.Models
{
    public class CoinTrend
    {
        public CoinTrend()
        {
            Time = DateTime.UtcNow;
            TimeDisplay = Time.ToLocalTime().ToString("G");
        }

        public double CurrentValue { get; set; }
        public int Trend { get; set; }

        [JsonIgnore]
        public DateTime Time { get; set; }

        [JsonIgnore]
        public string TimeDisplay { get; set; }

        public override string ToString()
        {
            return $"Value: {CurrentValue}";
        }
    }
}

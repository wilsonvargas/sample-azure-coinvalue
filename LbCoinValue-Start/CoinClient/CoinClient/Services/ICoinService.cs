using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoinClient.Models;

namespace CoinClient.Services
{
    public interface ICoinService
    {
        Task<CoinTrend> GetTrend();
    }
}

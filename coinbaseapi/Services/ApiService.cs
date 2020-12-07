using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using coinbaseapi.Hubs;
using coinbaseapi.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace coinbaseapi.Services
{
    public class ApiService : IApiService
    {
        private HttpClient _httpClient;
        private int _pollingInterval;
        private IMemoryCache _cache;
        private IHubContext<BTCHub> _hubContext;

        public ApiService(
          HttpClient httpClient,
          IMemoryCache cache,
          IHubContext<BTCHub> hubContext)
        {
            _httpClient = httpClient;
            _pollingInterval = 100000; //10 Seconds
            _cache = cache;
            _hubContext = hubContext;
        }

        public async Task<List<Currency>> GetCurrectPrices()
        {
            //Nomics API
            var response = await _httpClient.GetStringAsync("https://api.nomics.com/v1/currencies/ticker?key=5ce312d2e2933fa64e048c32ab9920e8&ids=BTC,ETH&interval=1h,1d,7d,30d,ytd&convert=NZD");
            //CurrentPricesResponse currentPrices = JsonConvert.DeserializeObject<CurrentPricesResponse>(response);

            return JsonConvert.DeserializeObject<List<Currency>>(response);
            //return new Price() { Value = currentPrice.BPI.NZD.RateFloat, Date = Convert.ToDateTime(currentPrice.Time.UpdatedIso) };
        }

        public async Task StartPollingCoindesk()
        {
            while (true)
            {
                List<Currency> currentPrice = await GetCurrectPrices();
                UpdatePricesInMemory(currentPrice);
                SendCurrentPriceToHub(currentPrice);
                Thread.Sleep(_pollingInterval);
            }
        }


        private void SendCurrentPriceToHub(List<Currency> currencies)
        {
            _hubContext.Clients.All.SendAsync("ReceivedCurrencies", currencies);
        }

        private void UpdatePricesInMemory(List<Currency> prices)
        {
            _cache.Set("PriceList", prices);
        }

        #region Removed
        // public async Task<Price> GetCurrentBtcPrice()
        // {
        //     var response = await _httpClient.GetStringAsync("https://api.coindesk.com/v1/bpi/currentprice/NZD");
        //     CurrentPriceResponse currentPrice = JsonConvert.DeserializeObject<CurrentPriceResponse>(response);
        //     return new Price() { Value = currentPrice.BPI.NZD.RateFloat, Date = Convert.ToDateTime(currentPrice.Time.UpdatedIso) };
        // }

        // public async Task StartPollingCoindesk()
        // {
        //     while (true)
        //     {
        //         Price currentPrice = await GetCurrentBtcPrice();
        //         AddPriceToListInMemory(currentPrice);
        //         SendCurrentPriceToHub(currentPrice);
        //         Thread.Sleep(_pollingInterval);
        //     }
        // }

        // private void SendCurrentPriceToHub(Price price)
        // {
        //     _hubContext.Clients.All.SendAsync("ReceivePrice", price);
        // }

        // private void AddPriceToListInMemory(Price price)
        // {
        //     IList<Price> priceList = _cache.Get("PriceList") as List<Price>;
        //     if (priceList == null)
        //     {
        //         _cache.CreateEntry("PriceList");
        //         _cache.Set("PriceList", new List<Price>() { price });
        //         priceList = _cache.Get("PriceList") as List<Price>;
        //     }

        //     if (priceList.Count == 5)
        //     {
        //         priceList.RemoveAt(0);
        //     }
        //     priceList.Add(price);
        //     _cache.Set("PriceList", priceList);
        // }

        #endregion

    }
}
using System.Threading.Tasks;
using coinbaseapi.Models;
using System.Collections.Generic;


public interface IApiService
{
    public Task<List<Currency>> GetCurrectPrices();
    // public Task<Price> GetCurrentBtcPrice();
    public Task StartPollingCoindesk();
}
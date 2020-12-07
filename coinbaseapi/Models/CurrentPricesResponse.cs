using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace coinbaseapi.Models
{

    public class Currency
    {
        public string id { get; set; }
        //public string currency { get; set; }
        public string name { get; set; }
        public string logo_url { get; set; }
        //public string status { get; set; }
        public string price { get; set; }
        public DateTime price_timestamp { get; set; }
        public string market_cap { get; set; }

        // [JsonProperty("1h")]
        // public Interval OneHour { get; set; }

        [JsonProperty("1h")]
        public IntervalResponse hour { get; set; }

        [JsonProperty("1d")]
        public IntervalResponse day { get; set; }

        [JsonProperty("7d")]
        public IntervalResponse week { get; set; }

        [JsonProperty("30d")]
        public IntervalResponse month { get; set; }

        [JsonProperty("60d")]
        public IntervalResponse quarter { get; set; }

        [JsonProperty("ytd")]
        public IntervalResponse year { get; set; }

    }


    public class IntervalResponse
    {
        public string volume { get; set; }
        public string price_change { get; set; }
        public string price_change_pct { get; set; }
        public string volume_change { get; set; }
        public string volume_change_pct { get; set; }
        public string market_cap_change { get; set; }
        public string market_cap_change_pct { get; set; }
    }

}
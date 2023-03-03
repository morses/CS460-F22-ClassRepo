using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;            // dotnet add package RestSharp
using Newtonsoft.Json;      // dotnet add package Newtonsoft.Json
using Newtonsoft.Json.Linq;

namespace AsyncDemo.Models
{
    public class ExchangeCandlestick
    {
        public int CloseTime { get; set; }
        public float OpenPrice { get; set; }
        public float HighPrice { get; set;}
        public float LowPrice { get; set;}
        public float ClosePrice { get; set;}    
        public float Volume { get; set;}
        public float QuoteVolume { get; set;}

    }

    public class CryptoWatchAPI
    {
        private readonly RestClient _client;

        public CryptoWatchAPI()
        {
            _client = new RestClient("https://api.cryptowat.ch/markets/")
                            .AddDefaultHeader(KnownHeaders.Accept, "application/json");
        }

        /*
         * https://docs.cryptowat.ch/rest-api/markets/ohlc
         * API returns the following
         * \{"result":{"3600":[[1676772000,24745.7,24770.5,24651.3,24716.4,53.56724412,1323505.678035565],[1676775600,24717.1,24735,24677.1,24690.6,16.35945224,404105.757266361],[1676779200,24690.6,24713,24663.5,24677.4,21.56858895,532354.350729991],[1676782800,24677.5,24717.2,24676.6,24717.1,12.38689291,305833.081699249],[1676786400,24717.2,24841.5,24717.1,24761.2,30.15441325,747503.775589849],[1676790000,24760.5,24764.6,24632.8,24634,34.61327081,854281.036262686],[1676793600,24634.5,24665.3,24575.1,24585,51.91940109,1278331.898485499],[1676797200,24584.6,24632.7,24575,24575,34.32380364,844746.188269793],[1676800800,24575.1,24628.9,24575,24602.8,15.4836284,380844.136590883],[1676804400,24603.9,24685.8,24590,24659.9,22.14253774,545728.562019773],[1676808000,24659,24704.9,24659,24679.3,32.19718855,794714.997186061],[1676811600,24679.4,24710.1,24670.9,24709.3,26.2744513,648857.955828011],[1676815200,24708.9,24717.1,24680,24682,20.10980244,496816.131712912],[1676818800,24682,24798,24680,24767.7,46.41797385,1148306.201630797],[1676822400,24765.1,25049,24752.7,24929.5,364.24943186,9074592.386107482],[1676826000,24930,25190,24670.1,24791,656.61654687,16407201.088076668],[1676829600,24791.1,24807.3,24313.7,24378.1,527.58578998,12948271.876558529],[1676833200,24361.6,24529.3,24296.3,24474.6,132.95622137,3246071.106014688],[1676836800,24469.6,24572.8,24466,24531,51.54460151,1264667.743603645],[1676840400,24531.1,24533.9,24251.2,24517.8,162.50097919,3962929.495891239],[1676844000,24517.7,24573.2,24491.3,24535,70.77893571,1736701.868970709],[1676847600,24535,24566.8,24457.7,24499.5,19.96891173,489663.380671049],[1676851200,24497.2,24497.2,24178.3,24287.2,307.11040474,7462828.172693435],[1676854800,24287.1,24405.1,24167.9,24223.1,235.11323155,5716172.672011966]]},"allowance":{"cost":0.015,"remaining":9.97,"upgrade":"For unlimited API access, create an account at https://cryptowat.ch"}}
          */

        public List<ExchangeCandlestick> GetHistoricalBitcoinData()
        {
            // Get prices over the past day, at 5 minute intervals, for Bitcoin in USD at the Kraken exchange
            DateTime now = DateTime.UtcNow;
            DateTime past = now.AddHours(-24);
            long after = ((DateTimeOffset)past).ToUnixTimeSeconds();
            long before = ((DateTimeOffset)now).ToUnixTimeSeconds();

            // Make the request
            RestRequest request = new RestRequest($"kraken/btcusd/ohlc?before={before}&after={after}&periods=300");
            // RestSharp v107 eliminated all their synchronous calls, so we block to simulate sync with the following
            RestResponse response = _client.GetAsync(request).GetAwaiter().GetResult();

            // Parse json using Newtonsoft and store in our model class
            JObject obj = JObject.Parse(response.Content);

            List<ExchangeCandlestick> exchangeCandlesticks = ((JArray)obj["result"]["300"]).Select(jarr => new ExchangeCandlestick
            {
                CloseTime = (int)jarr[0],
                OpenPrice = (float)jarr[1],
                HighPrice = (float)jarr[2],
                LowPrice = (float)jarr[3],
                ClosePrice = (float)jarr[4],
                Volume = (float)jarr[5],
                QuoteVolume = (float)jarr[6]
            }).ToList();
            Thread.Sleep(500);
            return exchangeCandlesticks;
        }

        public async Task<List<ExchangeCandlestick>> GetHistoricalBitcoinDataAsync()
        {
            // Get prices over the past day, at 5 minute intervals, for Bitcoin in USD at the Kraken exchange
            DateTime now = DateTime.UtcNow;
            DateTime past = now.AddHours(-24);
            long after = ((DateTimeOffset)past).ToUnixTimeSeconds();
            long before = ((DateTimeOffset)now).ToUnixTimeSeconds();

            // Make the request
            RestRequest request = new RestRequest($"kraken/btcusd/ohlc?before={before}&after={after}&periods=300");
            // RestSharp v107 eliminated all their synchronous calls, so we block to simulate sync with the following
            RestResponse response = await _client.GetAsync(request);

            // Parse json using Newtonsoft and store in our model class
            JObject obj = JObject.Parse(response.Content);

            List<ExchangeCandlestick> exchangeCandlesticks = ((JArray)obj["result"]["300"]).Select(jarr => new ExchangeCandlestick
            {
                CloseTime = (int)jarr[0],
                OpenPrice = (float)jarr[1],
                HighPrice = (float)jarr[2],
                LowPrice = (float)jarr[3],
                ClosePrice = (float)jarr[4],
                Volume = (float)jarr[5],
                QuoteVolume = (float)jarr[6]
            }).ToList();
            Thread.Sleep(500);
            return exchangeCandlesticks;
        }
    }

    // Model class
    public class BitcoinPriceIndex
    {
        public List<DateTime> Days { get; set; } = new List<DateTime>();
        public List<double> ClosingPrices { get; set; } = new List<double>();
        public string Disclaimer { get; set; } = String.Empty;
    }

    /*
     * Coindesk isn't responding to these requests anymore.  Found another API to use above.  Will switch over to that.
     */
    public class CoinDeskAPI
    {
        private readonly RestClient _client;

        public CoinDeskAPI()
        {
            _client = new RestClient("https://api.coindesk.com/v1")
                            .AddDefaultHeader(KnownHeaders.Accept, "application/json");
        }

        /*
         * API returns the following (note that the prices are in an object rather than an array, making it much
         * harder to extract)
         * {"bpi":{"2021-01-19":35929.1683,"2021-01-20":35510.67,"2021-01-21":30838.5,"2021-01-22":33018.825,"2021-01-23":32106.7033,"2021-01-24":32297.165,"2021-01-25":32255.35,"2021-01-26":32518.3583,"2021-01-27":30425.3933,"2021-01-28":33420.045,"2021-01-29":34264.01,"2021-01-30":34324.2717,"2021-01-31":33129.7433,"2021-02-01":33543.77,"2021-02-02":35528.31,"2021-02-03":37685.2767,"2021-02-04":36984.6783,"2021-02-05":38306.2467,"2021-02-06":39269.3417,"2021-02-07":38862.35,"2021-02-08":46436.09,"2021-02-09":46502.2933,"2021-02-10":44855.6167,"2021-02-11":48004.6533,"2021-02-12":47410.4033,"2021-02-13":47211.6683,"2021-02-14":48633.26,"2021-02-15":47934.1267,"2021-02-16":49185.7283,"2021-02-17":52127.32,"2021-02-18":51573.4067},"disclaimer":"This data was produced from the CoinDesk Bitcoin Price Index. BPI value data returned as USD.","time":{"updated":"Feb 19, 2021 00:03:00 UTC","updatedISO":"2021-02-19T00:03:00+00:00"}}
         **/

        public BitcoinPriceIndex GetBPI()
        {
            // Make the request
            RestRequest request = new RestRequest("bpi/currentprice.json");
            // RestSharp v107 eliminated all their synchronous calls, so we block to simulate sync with the following
            RestResponse response = _client.GetAsync(request).GetAwaiter().GetResult();

            // Parse json using Newtonsoft and store in our model class
            JObject obj = JObject.Parse(response.Content);
            BitcoinPriceIndex bpi = new BitcoinPriceIndex();
            bpi.Disclaimer = (string)obj["disclaimer"];
            var priceDict = obj["bpi"].ToObject<Dictionary<string,double>>();
            foreach(KeyValuePair<string,double> entry in priceDict)
            {
                bpi.Days.Add(DateTime.Parse(entry.Key));
                bpi.ClosingPrices.Add(entry.Value);
            }
            Thread.Sleep(500);
            return bpi;
        }

        public async Task<BitcoinPriceIndex> GetBPIAsync()
        {
            // Make the request using Async method
            RestRequest request = new RestRequest("bpi/historical/close.json");
            RestResponse response = await _client.GetAsync(request);

            // Parse json using Newtonsoft and store in our model class
            JObject obj = JObject.Parse(response.Content);
            BitcoinPriceIndex bpi = new BitcoinPriceIndex();
            bpi.Disclaimer = (string)obj["disclaimer"];
            var priceDict = obj["bpi"].ToObject<Dictionary<string,double>>();
            foreach(KeyValuePair<string,double> entry in priceDict)
            {
                bpi.Days.Add(DateTime.Parse(entry.Key));
                bpi.ClosingPrices.Add(entry.Value);
            }
            Thread.Sleep(500);
            return bpi;
        }


    }
}

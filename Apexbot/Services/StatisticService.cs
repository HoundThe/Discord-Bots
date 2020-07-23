using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using ApexBot.Services.Model;
using Microsoft.Extensions.Configuration;

namespace ApexBot.Services
{
    public class StatisticService
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        
        private readonly string _getUrl;

        public StatisticService(IConfigurationRoot config)
        {
            _getUrl = config["ApiGetUrl"];
            HttpClient.DefaultRequestHeaders.Add("TRN-Api-Key", config["ApiKey"]);
        }
        public async Task<StatisticsModel> RequestStats(string name)
        {
            string getRequest = $"{_getUrl}{name}";
            getRequest = getRequest.ToLower();

            try
            {
                using var response = await HttpClient.GetAsync(getRequest).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    throw new NullReferenceException("***Request Failed***, \n*Wrong nickname?*\n(Bot uses your server username or your nickname if you have one)");
                }

                var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var result = JsonConvert.DeserializeObject<StatisticsModel>(responseData);

                return result;
            }
            catch (JsonException e)
            {
                Console.WriteLine(e.Message);
                throw new Exception("Parsing error");
            }
        }

    }
}

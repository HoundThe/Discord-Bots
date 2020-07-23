using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ApexBot.Services;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PUBGBot.Models;

namespace PUBGBot.Services
{
    public class BattlegroundsService
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private static readonly HtmlWeb HtmlWeb = new HtmlWeb();
        private readonly Config _config;

        public BattlegroundsService(Config config)
        {
            _config = config;

            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _config.ApiKey);
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Fetches player stats from pubg.op.gg
        /// </summary>
        /// <param name="name">in-game nickname</param>
        /// <returns></returns>
        public async Task<PlayerModel> GetPlayerStats(string name)
        {
            var getRequest = _config.ApiPlayerUrl + name;
            
            try
            {
                using var response = await HttpClient.GetAsync(getRequest).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.TooManyRequests)
                    {
                        throw new HttpRequestException("***Request Failed***, \n*Too many requests!*");
                    }
                    throw new HttpRequestException("***Request Failed***, \n*No games played. Wrong nickname?*");
                }
                // need to get the id of a player to query futher
                var jsonResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var parsedObj = JObject.Parse(jsonResponse);
                var id = parsedObj["data"][0]["id"];

                getRequest = _config.ApiPlayerStatsUrl + id + "/seasons/" + _config.Season;

                using var nextResponse = await HttpClient.GetAsync(getRequest).ConfigureAwait(false);
                {
                    if (!nextResponse.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.TooManyRequests)
                        {
                            throw new HttpRequestException("***Request Failed***, \n*Too many requests!*");
                        }
                        throw new HttpRequestException("***Request Failed***, \n*No games played. Wrong nickname?*");
                    }

                    jsonResponse = await nextResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var result = JsonConvert.DeserializeObject<PlayerModel>(jsonResponse);
                    
                    return result;
                }
            }
            catch (JsonException e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
        }
    }
}
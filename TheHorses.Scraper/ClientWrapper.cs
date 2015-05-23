using System;
using System.Net.Http;

namespace TheHorses.Scraper
{
    public class ClientWrapper
    {
        private HttpClient _httpClient;

        public ClientWrapper(string url)
        {
            _httpClient = new HttpClient() {BaseAddress = new Uri(url)};
            var r = _httpClient.GetAsync("").Result.Content.ReadAsStringAsync().Result;
        }

    }
}
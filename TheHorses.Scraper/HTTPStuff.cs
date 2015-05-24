using System.Net.Http;
using System.Threading.Tasks;

namespace TheHorses.Scraper
{
    public class HTTPStuff
    {
        public static async Task<string> DownloadPageAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2224.3 Safari/537.36");

                using (HttpResponseMessage response = await client.GetAsync(url))
                using (HttpContent content = response.Content)
                    return await content.ReadAsStringAsync();
            }
        }
    }
}
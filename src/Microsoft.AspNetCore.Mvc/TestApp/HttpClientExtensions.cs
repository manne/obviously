using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestApp
{
    public static class HttpClientExtensions
    {
        public static async Task<T> GetAsync<T>(this HttpClient instance, string url)
        {
            using var httpResponseMessage = await instance.GetAsync(url);
            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            var deserializeObject = JsonConvert.DeserializeObject<T>(content);
            return deserializeObject;
        }
    }
}

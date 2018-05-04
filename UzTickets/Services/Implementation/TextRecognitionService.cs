using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UzTickets.Helpers;
using UzTickets.Models;

namespace UzTickets.Services.Implementation
{
    public class TextRecognitionService : ITextRecognitionService
    {
        readonly string LuisBaseUrl = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/{0}?subscription-key={1}&verbose=true&timezoneOffset=0&q={2}";

        readonly IConfiguration _configuration;
        readonly HttpClient _httpClient;

        public TextRecognitionService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<RecognizedMessage> AnalyzeMessageAsync(string message)
        {
            var result = new RecognizedMessage();

            var requestUrl = GetLuisRequestUrl(message);

            using (var stream = await _httpClient.GetStreamAsync(requestUrl))
            {
                var response = stream.To<Luis.Response>();

                var entities = response.Entities;

                result.Day = entities.FirstOrDefault(e => e.Type == "Day")?.Name;
                result.Month = entities.FirstOrDefault(e => e.Type == "Month")?.Name;
                result.From = entities.FirstOrDefault(e => e.Type == "From")?.Name;
                result.To = entities.FirstOrDefault(e => e.Type == "To")?.Name;
            }

            return result;
        }

        private string GetLuisRequestUrl(string query)
        {
            return string.Format(LuisBaseUrl, _configuration["luisAppId"], _configuration["luidAppKey"], query);
        }
    }
}

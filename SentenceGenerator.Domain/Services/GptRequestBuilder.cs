using Microsoft.Extensions.Options;
using SentenceGenerator.Domain.Models;

namespace SentenceGenerator.Domain.Services
{
    public class GptRequestBuilder : IGptRequestBuilder
    {
        private readonly AppSettings _appSettings;

        public GptRequestBuilder(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public GptRequest Build(string keyword)
        {
            var message = new GptRequestMessage
            {
                Role = "user",
                Content = $"write a sentence about {keyword}"
            };

            var request = new GptRequest
            {
                Model = _appSettings.GptModel,
                Messages = new List<GptRequestMessage> { message },
                Temperature = _appSettings.GptTemperature
            };

            return request;
        }
    }
}

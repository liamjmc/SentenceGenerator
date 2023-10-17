using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;
using Google.Api.Gax.Grpc.Rest;
using Google.Cloud.Language.V1;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using SentenceGenerator.Domain.Models;
using System.Security.Principal;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;

namespace SentenceGenerator.Domain
{
    public class GptRequester : IGptRequester
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public readonly AppSettings _appSettings;
        public readonly ILogger<GptRequester> _logger;

        public GptRequester(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings, ILogger<GptRequester> logger)
        {
            _httpClientFactory = httpClientFactory;
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        public async Task<string> GetAsync(string keyword, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                throw new ArgumentNullException(nameof(keyword));
            }

            var message = new GptRequestMessage
            {
                Role = "user",
                Content = $"write a sentence about {keyword}"
            };

            var request = new GptRequest
            {
                Model = "gpt-3.5-turbo",
                Messages = new List<GptRequestMessage> { message },
                Temperature = 0.7
            };

            var requestiItemJson = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                Application.Json);

            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Post,
                "https://api.openai.com/v1/chat/completions")
            {
                Headers = { { HeaderNames.Accept, "application/json" } },
                Content = requestiItemJson
            };

            var httpClient = _httpClientFactory.CreateClient();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _appSettings.ClientSecret);

            try
            {
                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, cancellationToken);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                    var response = await JsonSerializer.DeserializeAsync<GptResponse>(contentStream);

                    return response.Choices.FirstOrDefault()?.Message.Content;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when trying to proxy request with keyword \"{keyword}\"");

                throw;
            }

            //TODO: should this throw an error?
            return null;
        }
    }
}
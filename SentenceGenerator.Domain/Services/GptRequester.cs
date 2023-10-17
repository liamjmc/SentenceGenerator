using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using SentenceGenerator.Domain.Models;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;

namespace SentenceGenerator.Domain.Services
{
    public class GptRequester : IGptRequester
    {
        private readonly IGptRequestBuilder _gptRequestBuilder;
        private readonly IHttpClientFactory _httpClientFactory;
        public readonly AppSettings _appSettings;
        public readonly ILogger<GptRequester> _logger;

        public GptRequester(IGptRequestBuilder gptRequestBuilder, IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings, ILogger<GptRequester> logger)
        {
            _gptRequestBuilder = gptRequestBuilder;
            _httpClientFactory = httpClientFactory;
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        public async Task<GptResponse> GetAsync(string keyword, CancellationToken cancellationToken)
        {
            var gptRequest = _gptRequestBuilder.Build(keyword);

            var requestiItemJson = new StringContent(
                JsonSerializer.Serialize(gptRequest),
                Encoding.UTF8,
                Application.Json);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, _appSettings.ClientUrl)
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

                    return await JsonSerializer.DeserializeAsync<GptResponse>(contentStream);
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
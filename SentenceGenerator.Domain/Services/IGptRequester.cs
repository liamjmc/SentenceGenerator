using SentenceGenerator.Domain.Models;

namespace SentenceGenerator.Domain.Services
{
    public interface IGptRequester
    {
        Task<GptResponse> GetAsync(string keyword, CancellationToken cancellationToken);
    }
}
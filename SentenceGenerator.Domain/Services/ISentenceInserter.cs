using SentenceGenerator.Domain.Models;

namespace SentenceGenerator.Domain.Services
{
    public interface ISentenceInserter
    {
        Task AddAsync(string keyword, GptResponse gptResponse);
    }
}
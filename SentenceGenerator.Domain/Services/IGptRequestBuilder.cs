using SentenceGenerator.Domain.Models;

namespace SentenceGenerator.Domain.Services
{
    public interface IGptRequestBuilder
    {
        GptRequest Build(string keyword);
    }
}
namespace SentenceGenerator.Domain.Services
{
    public interface ISentenceGenerator
    {
        Task<string> ProcessAsync(string keyword, CancellationToken cancellationToken);
    }
}
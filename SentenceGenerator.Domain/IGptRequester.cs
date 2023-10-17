namespace SentenceGenerator.Domain
{
    public interface IGptRequester
    {
        Task<string> GetAsync(string keyword, CancellationToken cancellationToken);
    }
}
using SentenceGenerator.DataAccess.Models;

namespace SentenceGenerator.DataAccess.Repository
{
    public interface ISentenceRepository
    {
        Task<List<Sentence>> GetSentences();
        Task<Sentence> CreateSentence(Sentence sentence, CancellationToken cancellationToken);
    }
}
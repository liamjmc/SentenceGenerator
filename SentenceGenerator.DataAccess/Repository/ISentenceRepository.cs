using SentenceGenerator.DataAccess.Models;

namespace SentenceGenerator.DataAccess.Repository
{
    public interface ISentenceRepository
    {
        Task<Sentence> CreateSentence(Sentence sentence);
        Task<List<Sentence>> GetSentences();
    }
}
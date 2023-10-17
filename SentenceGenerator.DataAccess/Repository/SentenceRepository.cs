using Microsoft.EntityFrameworkCore;
using SentenceGenerator.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentenceGenerator.DataAccess.Repository
{
    public class SentenceRepository : ISentenceRepository
    {
        public async Task<List<Sentence>> GetSentences()
        {
            using (var context = new ApplicationDbContext())
            {
                return await context.Sentences.ToListAsync();
            }
        }

        public async Task<Sentence> CreateSentence(Sentence sentence)
        {
            using (var context = new ApplicationDbContext())
            {
                await context.Sentences.AddAsync(sentence);

                await context.SaveChangesAsync();

                return sentence;
            }
        }
    }
}

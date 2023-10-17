using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentenceGenerator.DataAccess.Models
{
    public class Sentence
    {
        public int Id { get; set; }
        public string Keyword { get; set; }
        public string Content { get; set; }
        public double Temperature { get; set; }
        public string GptModel { get; set; }
        public int Created { get; set; }
    }
}

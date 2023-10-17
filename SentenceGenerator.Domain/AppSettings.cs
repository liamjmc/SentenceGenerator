using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentenceGenerator.Domain
{
    public class AppSettings
    {
        public string ClientUrl { get; set; }  
        public string ClientSecret { get; set; }
        public string GptModel { get; set; }
        public double GptTemperature { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using SentenceGenerator.DataAccess.Models;
using SentenceGenerator.DataAccess.Repository;
using SentenceGenerator.Domain.Services;

namespace SentenceGenerator.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SentenceController : ControllerBase
    {
        private readonly ISentenceGenerator _sentenceGenerator;
        private readonly ISentenceRepository _sentenceRepository;
        private readonly ILogger<SentenceController> _logger;

        public SentenceController(ISentenceGenerator sentenceGenerator, ISentenceRepository sentenceRepository, ILogger<SentenceController> logger)
        {
            _sentenceGenerator = sentenceGenerator;
            _sentenceRepository = sentenceRepository;
            _logger = logger;
        }

        [HttpGet("get-all", Name = "GetAllSentences")]
        public async Task<ActionResult<IEnumerable<Sentence>>> GetAll()
        {
            var result = await _sentenceRepository.GetSentences();

            return Ok(result);
        }

        //todo maybe by route instead?
        [HttpPost(Name = "GenerateSentence")]
        public async Task<ActionResult<string>> GenerateSentence([FromQuery] string keyword, CancellationToken cancellationToken)
        {
            var result = await _sentenceGenerator.ProcessAsync(keyword, cancellationToken);

            return Ok(result);
        }
    }
}
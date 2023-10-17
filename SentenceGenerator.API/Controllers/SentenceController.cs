using Microsoft.AspNetCore.Mvc;
using SentenceGenerator.Domain;

namespace SentenceGenerator.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SentenceController : ControllerBase
    {
        private readonly IGptRequester _gptRequester;
        private readonly ILogger<SentenceController> _logger;

        public SentenceController(IGptRequester gptRequester, ILogger<SentenceController> logger)
        {
            _gptRequester = gptRequester;
            _logger = logger;
        }

        //todo maybe by route instead?
        [HttpPost(Name = "GenerateSentence")]
        public async Task<string> GenerateSentence([FromQuery] string keyword, CancellationToken cancellationToken)
        {
            return await _gptRequester.GetAsync(keyword, cancellationToken);
        }
    }
}
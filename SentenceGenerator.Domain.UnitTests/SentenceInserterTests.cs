using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SentenceGenerator.DataAccess.Models;
using SentenceGenerator.DataAccess.Repository;
using SentenceGenerator.Domain.Models;
using SentenceGenerator.Domain.Services;

namespace SentenceGenerator.Domain.UnitTests
{
    public class SentenceInserterTests
    {
        private Mock<ISentenceRepository> _sentenceRepository;
        private SentenceInserter _sentenceInserter;

        [SetUp]
        public void Setup()
        {
            _sentenceRepository = new Mock<ISentenceRepository>();

            var logger = new Mock<ILogger<SentenceInserter>>();

            var appSettings = Options.Create(new AppSettings { GptModel = "gpt-9", GptTemperature = 0.9 });

            _sentenceInserter = new SentenceInserter(_sentenceRepository.Object, appSettings, logger.Object);
        }

        [TestCase("   ")]
        [TestCase("")]
        [TestCase(null)]
        public async Task GivenASenteceInsertionIsRequested_WhenTheContentIsNullOrWhiteSpace_ThenTheSentenceIsNotInserted(string sentence)
        {
            var gptResponseMessage = new GptResponseMessage { Content = sentence };

            var gptResponse = new GptResponse
            {
                Choices = new List<GptResponseChoice> { new GptResponseChoice { Message = gptResponseMessage } }
            };

            await _sentenceInserter.AddAsync("valid", gptResponse, CancellationToken.None);

            _sentenceRepository.Verify(s => s.CreateSentence(It.IsAny<Sentence>(), CancellationToken.None), Times.Never);
        }

        [Test]
        public async Task GivenASenteceInsertionIsRequested_WhenTheContentIsValid_ThenTheSentenceIsInserted()
        {
            var gptResponseMessage = new GptResponseMessage { Content = "There is an issue with completing the registration process." };

            var gptResponse = new GptResponse
            {
                Choices = new List<GptResponseChoice> { new GptResponseChoice { Message = gptResponseMessage } }
            };

            await _sentenceInserter.AddAsync("valid", gptResponse, CancellationToken.None);

            _sentenceRepository.Verify(s => 
                    s.CreateSentence(It.Is<Sentence>(s => s.Content == "There is an issue with completing the registration process."), 
                CancellationToken.None), Times.Once);
        }
    }
}
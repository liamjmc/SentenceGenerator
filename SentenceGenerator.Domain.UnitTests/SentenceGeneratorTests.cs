using Moq;
using SentenceGenerator.Domain.Models;
using SentenceGenerator.Domain.Services;

namespace SentenceGenerator.Domain.UnitTests
{
    public class SentenceGeneratorTests
    {
        private Mock<IGptRequester> _gptRequesterMock;
        private Mock<ISentenceInserter> _sentenceInserterMock;
        private SentenceGenerator.Domain.Services.SentenceGenerator _sentenceGenerator;

        [SetUp]
        public void Setup()
        {
            _gptRequesterMock = new Mock<IGptRequester>();
            _sentenceInserterMock = new Mock<ISentenceInserter>();
            _sentenceGenerator = new SentenceGenerator.Domain.Services.SentenceGenerator(_gptRequesterMock.Object, _sentenceInserterMock.Object);
        }

        [TestCase("registration")]
        [TestCase("bank")]
        [TestCase("website")]
        public async Task GivenASenteceGenerationIsRequested_WhenTheKeywordIsValid_ThenGptIsRequested(string keyword)
        {
            var gptResponseMessage = new GptResponseMessage { Content = string.Empty };

            var gptResponse = new GptResponse
            {
                Choices = new List<GptResponseChoice> { new GptResponseChoice { Message = gptResponseMessage } }
            };

            _gptRequesterMock.Setup(g => g.GetAsync(keyword, CancellationToken.None)).ReturnsAsync(gptResponse);

            await _sentenceGenerator.ProcessAsync(keyword, CancellationToken.None);

            _gptRequesterMock.Verify(g => g.GetAsync(keyword, CancellationToken.None), Times.Once());
        }

        [TestCase("registration")]
        [TestCase("bank")]
        [TestCase("website")]
        public async Task GivenASenteceGenerationIsRequested_WhenTheKeywordIsValid_ThenTheSentenceIsInserted(string keyword)
        {
            var gptResponseMessage = new GptResponseMessage { Content = string.Empty };

            var gptResponse = new GptResponse
            {
                Choices = new List<GptResponseChoice> { new GptResponseChoice { Message = gptResponseMessage } }
            };

            _gptRequesterMock.Setup(g => g.GetAsync(keyword, CancellationToken.None)).ReturnsAsync(gptResponse);

            await _sentenceGenerator.ProcessAsync(keyword, CancellationToken.None);

            _sentenceInserterMock.Verify(g => g.AddAsync(keyword, It.IsAny<GptResponse>(), CancellationToken.None), Times.Once());
        }

        [Test]
        public void GivenASenteceGenerationIsRequested_WhenTheGptRequesterThrowsAnError_ThenAnErrorIsThrown()
        {
            _gptRequesterMock.Setup(g => g.GetAsync("registration", CancellationToken.None))
                .ThrowsAsync(new ArgumentException());

            Assert.That(async () => await _sentenceGenerator.ProcessAsync("registration", CancellationToken.None), Throws.ArgumentException);
        }

        [Test]
        public void GivenASenteceGenerationIsRequested_WhenTheSentenceInserterThrowsAnError_ThenAnErrorIsThrown()
        {
            _sentenceInserterMock.Setup(s => s.AddAsync("registration", It.IsAny<GptResponse>(), CancellationToken.None))
                .ThrowsAsync(new ArgumentException());

            Assert.That(async () => await _sentenceGenerator.ProcessAsync("registration", CancellationToken.None), Throws.ArgumentException);
        }

        [TestCase]
        public async Task GivenASentenceGenerationIsRequested_WhenASentenceIsReturnedFromTheRequester_ThenASentenceIsReturned()
        {
            var gptResponseMessage = new GptResponseMessage { Content = "I have a problem initiating the registration flow at your platform." };

            var gptResponse = new GptResponse
            {
                Choices = new List<GptResponseChoice> { new GptResponseChoice { Message = gptResponseMessage } }
            };

            _gptRequesterMock.Setup(g => g.GetAsync("registration", CancellationToken.None)).ReturnsAsync(gptResponse);

            var result = await _sentenceGenerator.ProcessAsync("registration", CancellationToken.None);

            Assert.That(result, Is.EqualTo("I have a problem initiating the registration flow at your platform."));
        }
    }
}
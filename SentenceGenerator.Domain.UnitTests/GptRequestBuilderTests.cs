using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SentenceGenerator.DataAccess.Models;
using SentenceGenerator.DataAccess.Repository;
using SentenceGenerator.Domain.Models;
using SentenceGenerator.Domain.Services;

namespace SentenceGenerator.Domain.UnitTests
{
    public class GptRequestBuilderTests
    {
        private GptRequestBuilder _gptRequestBuilder;

        [SetUp]
        public void Setup()
        {
            var appSettings = Options.Create(new AppSettings { GptModel = "gpt-3.5", GptTemperature = 0.65 });

            _gptRequestBuilder = new GptRequestBuilder(appSettings);
        }

        [TestCase("registration")]
        [TestCase("bank")]
        [TestCase("website")]
        public async Task GivenABuildRequestIsRequested_WhenTheDataIsValid_ThenAGptRequestIsReturned(string keyword)
        {
            var result = _gptRequestBuilder.Build(keyword);

            Assert.That(result.Model, Is.EqualTo("gpt-3.5"));
            Assert.That(result.Temperature, Is.EqualTo(0.65));
            Assert.That(result.Messages.First().Content, Is.EqualTo($"write a sentence about {keyword}"));
            Assert.That(result.Messages.First().Role, Is.EqualTo("user"));
        }
    }
}
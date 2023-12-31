﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SentenceGenerator.DataAccess.Models;
using SentenceGenerator.DataAccess.Repository;
using SentenceGenerator.Domain.Models;
using System.Text.Json;

namespace SentenceGenerator.Domain.Services
{
    public class SentenceInserter : ISentenceInserter
    {
        private ISentenceRepository _sentenceRepository;
        private readonly AppSettings _appSettings;
        public readonly ILogger<SentenceInserter> _logger;

        public SentenceInserter(ISentenceRepository sentenceRepository, IOptions<AppSettings> appSettings, ILogger<SentenceInserter> logger)
        {
            _sentenceRepository = sentenceRepository;
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        public async Task AddAsync(string keyword, GptResponse gptResponse, CancellationToken cancellationToken)
        {
            var sentenceContent = gptResponse.Choices.FirstOrDefault()?.Message.Content;

            if (string.IsNullOrWhiteSpace(sentenceContent)) return;

            var sentence = new Sentence
            {
                Keyword = keyword,
                Created = gptResponse.Created,
                Content = sentenceContent,
                GptModel = _appSettings.GptModel,
                Temperature = _appSettings.GptTemperature
            };

            try
            {
                await _sentenceRepository.CreateSentence(sentence, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inserting sentence into the database for keyword {keyword}. Response: {JsonSerializer.Serialize(gptResponse)}");
                throw;
            }
        }
    }
}

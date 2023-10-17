namespace SentenceGenerator.Domain.Services
{
    public class SentenceGenerator : ISentenceGenerator
    {
        private readonly IGptRequester _gptRequester;
        private readonly ISentenceInserter _sentenceInserter;

        public SentenceGenerator(IGptRequester gptRequester, ISentenceInserter sentenceInserter)
        {
            _gptRequester = gptRequester;
            _sentenceInserter = sentenceInserter;
        }

        public async Task<string> ProcessAsync(string keyword, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                throw new ArgumentNullException(nameof(keyword));
            }

            var gptResponse = await _gptRequester.GetAsync(keyword, cancellationToken);

            await _sentenceInserter.AddAsync(keyword, gptResponse, cancellationToken);

            return gptResponse.Choices.FirstOrDefault()?.Message.Content;
        }
    }
}
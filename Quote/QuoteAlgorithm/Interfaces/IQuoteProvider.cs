using QuoteAlgorithm.Entities;

namespace QuoteAlgorithm.Interfaces
{
    public interface IQuoteProvider
    {
        Quote CreateQuote(string marketFilePath, string loanAmount);
    }
}
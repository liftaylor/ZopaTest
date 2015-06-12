using System.Collections.Generic;
using QuoteAlgorithm.Entities;

namespace QuoteAlgorithm.Interfaces
{
    public interface ISourceProvider
    {
        IList<Offer> GetSource(string marketFilePath);
    }
}
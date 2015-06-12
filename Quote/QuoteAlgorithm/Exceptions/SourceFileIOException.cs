using System;

namespace QuoteAlgorithm.Exceptions
{
    public class SourceFileIOException : Exception
    {
        public SourceFileIOException(string message) 
            : base(message)
        {
        }
    }
}
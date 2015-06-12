using System;

namespace QuoteAlgorithm.Exceptions
{
    public class SourceFileFormatException : Exception
    {
        public SourceFileFormatException(string message) 
            : base(message)
        {
        }
    }
}
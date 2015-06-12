using System;

namespace QuoteAlgorithm.Exceptions
{
    public class LoanAmountException : Exception
    {
        public LoanAmountException(string message): 
            base(message)
        {
        }
    }
}
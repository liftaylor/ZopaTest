using System;
using System.Linq;
using QuoteAlgorithm.Entities;
using QuoteAlgorithm.Exceptions;
using QuoteAlgorithm.Interfaces;

namespace QuoteAlgorithm
{
    public class QuoteProvider : IQuoteProvider
    {
        private readonly ISourceProvider _sourceProvider;

        public QuoteProvider(ISourceProvider sourceProvider)
        {
            _sourceProvider = sourceProvider;
        }

        public Quote CreateQuote(string marketFilePath, string loanAmountString)
        {
            long loanAmount;
            if (!long.TryParse(loanAmountString, out loanAmount))
            {
                throw new LoanAmountException("Please input an integer loan amount.");
            }
            var offers = _sourceProvider.GetSource(marketFilePath);
            if (loanAmount < 1000 || loanAmount > 15000 || loanAmount%100 != 0)
            {
                throw new LoanAmountException(
                    "Load request can only be of any £100 increment between £1000 and £15000 inclusive.");
            }
            if (offers.Sum(o => o.Available) < loanAmount)
            {
                throw new LoanAmountException("It is not possible to provide a quote at that time.");
            }

            var usableTotalAmount = 0L;

            var offersToUse = offers.TakeWhile(o =>
            {
                var available = o.Available;
                usableTotalAmount += available;
                return (usableTotalAmount < loanAmount);
            }).ToList();

            offersToUse.Add(offers[offersToUse.Count]);

            var sumAvailable = offersToUse.Sum(o => o.Available);

            var compoundRate = (from offer in offersToUse let weight = (double) offer.Available/sumAvailable select offer.Rate*weight).Sum();
            compoundRate = Math.Round(compoundRate, 3);
            var finalRate = Math.Pow(compoundRate + 1, 3);
            var totalPayment = finalRate*loanAmount;
            var monthlyPayment = totalPayment / 36;

            return new Quote
            {
                Rate = compoundRate ,
                MonthlyRepayment = Math.Round(monthlyPayment, 2),
                RequestAmount = loanAmount,
                TotalRepayment = Math.Round(totalPayment, 2)
            };
        }
    }
}
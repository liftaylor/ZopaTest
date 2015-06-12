using System.Collections.Generic;
using System.Linq;
using FluentAssert;
using NSubstitute;
using NUnit.Framework;
using QuoteAlgorithm;
using QuoteAlgorithm.Entities;
using QuoteAlgorithm.Exceptions;
using QuoteAlgorithm.Interfaces;

namespace QuoteAlgorithmTests
{
    [TestFixture]
    public class QuoteProviderShould
    {
        private ISourceProvider _sourceProvider;
        private IQuoteProvider _quoteProvider;
        private IList<Offer> _offers;

        [TestFixtureSetUp]
        public void Setup()
        {
            _sourceProvider = Substitute.For<ISourceProvider>();
            _quoteProvider = new QuoteProvider(_sourceProvider);
            _offers = new []
            {
                new Offer {Lender = "Bob", Rate = 0.075, Available = 640},
                new Offer {Lender = "Jane", Rate = 0.069, Available = 480},
                new Offer {Lender = "Fred", Rate = 0.071, Available = 520},
                new Offer {Lender = "Mary", Rate = 0.104, Available = 170},
                new Offer {Lender = "John", Rate = 0.081, Available = 320},
                new Offer {Lender = "Dave", Rate = 0.074, Available = 140},
                new Offer {Lender = "Angela", Rate = 0.071, Available = 60}
            }
            .OrderBy(o => o.Rate)
            .ToList();
        }

        [TestFixtureTearDown]
        public void CleanUp()
        {
            _sourceProvider = null;
            _quoteProvider = null;
            _offers.Clear();
            _offers = null;
        }

        [Test]
        public void GiveSuchAQuoteWhenLoanAmountIs2100()
        {
            var filePath = string.Empty;
            var expectedQuote = new Quote
            {
                RequestAmount = 2100,
                Rate = 0.073,
                MonthlyRepayment = 72.06,
                TotalRepayment = 2594.29
            };
            _sourceProvider.GetSource(filePath).ReturnsForAnyArgs(_offers);
            _quoteProvider.CreateQuote(filePath, "2100").ShouldBeEqualTo(expectedQuote);
            _sourceProvider.Received(1).GetSource(filePath);
        }

        [Test]
        [ExpectedException(typeof(LoanAmountException), ExpectedMessage = "Load request can only be of any £100 increment between £1000 and £15000 inclusive.")]
        public void ThrowLoanAmountExceptionIfLoanAmountIsLargerThan15000()
        {
            _sourceProvider.GetSource(Arg.Any<string>()).ReturnsForAnyArgs(_offers);
            _quoteProvider.CreateQuote(Arg.Any<string>(), "15100");
        }

        [Test]
        [ExpectedException(typeof(LoanAmountException), ExpectedMessage = "It is not possible to provide a quote at that time.")]
        public void ThrowLoanAmountExceptionIfLoanAmountIsLargerThanAvailable()
        {
            _sourceProvider.GetSource(Arg.Any<string>()).ReturnsForAnyArgs(_offers);
            _quoteProvider.CreateQuote(Arg.Any<string>(), "2500");
        }

        [Test]
        [ExpectedException(typeof(LoanAmountException), ExpectedMessage = "Please input an integer loan amount.")]
        public void ThrowLoanAmountExceptionIfLoanAmountIsNotInteger()
        {
            _sourceProvider.GetSource(Arg.Any<string>()).ReturnsForAnyArgs(_offers);
            _quoteProvider.CreateQuote(Arg.Any<string>(), "2000.1");
        }

        [Test]
        [ExpectedException(typeof(LoanAmountException), ExpectedMessage = "Load request can only be of any £100 increment between £1000 and £15000 inclusive.")]
        public void ThrowLoanAmountExceptionIfLoanAmountIsSmallerThan1000()
        {
            _sourceProvider.GetSource(Arg.Any<string>()).ReturnsForAnyArgs(_offers);
            _quoteProvider.CreateQuote(Arg.Any<string>(), "900");
        }

        [Test]
        [ExpectedException(typeof(LoanAmountException), ExpectedMessage = "Load request can only be of any £100 increment between £1000 and £15000 inclusive.")]
        public void ThrowLoanAmountExceptionIfLoanAmountModuloBy100IsNotZero()
        {
            _sourceProvider.GetSource(Arg.Any<string>()).ReturnsForAnyArgs(_offers);
            _quoteProvider.CreateQuote(Arg.Any<string>(), "1310");
        }
    }
}
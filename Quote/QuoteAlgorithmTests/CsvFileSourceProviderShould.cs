using FluentAssert;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using QuoteAlgorithm;
using QuoteAlgorithm.Exceptions;
using QuoteAlgorithm.Interfaces;

namespace QuoteAlgorithmTests
{
    [TestFixture]
    public class CsvFileSourceProviderShould
    {
        private ISourceProvider _target;
        private const string TestFileFolder = "Test Data";
        private const string ValidFilePath = @"Test Data\Market.csv";
        private const string InvalidFilePath = @"Invalid Test Data\Market.csv";
        private const string ErrorFormatFilePath = @"Test Data\Invalid Market.csv";

        [TestFixtureSetUp]
        public void Setup()
        {
            _target = new CsvFileSourceProvider();
        }

        [TestFixtureTearDown]
        public void CleanUp()
        {
            _target = null;
        }

        [Test]
        [DeploymentItem(ValidFilePath, TestFileFolder)]
        [NUnit.Framework.ExpectedException(typeof(SourceFileIOException), ExpectedMessage = "File path doesn't exist.")]
        public void ThrowSourceFileIoExceptionIfFilePathIsInvalid()
        {
            _target.GetSource(InvalidFilePath);
        }

        [Test]
        [DeploymentItem(ErrorFormatFilePath, TestFileFolder)]
        [NUnit.Framework.ExpectedException(typeof(SourceFileFormatException), ExpectedMessage = "This market file format is invalid!")]
        public void ThrowSourceFileFormatExceptionIfFileFormatIsInvalid()
        {
            _target.GetSource(ErrorFormatFilePath);
        }

        [Test]
        [DeploymentItem(ValidFilePath, TestFileFolder)]
        public void Return7OffersIfFilePathAndFormatAreValid()
        {
            var offers = _target.GetSource(ValidFilePath);
            offers.Count.ShouldBeEqualTo(7);
        }

    }
}
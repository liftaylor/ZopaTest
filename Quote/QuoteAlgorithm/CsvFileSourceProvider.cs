using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using QuoteAlgorithm.Entities;
using QuoteAlgorithm.Exceptions;
using QuoteAlgorithm.Interfaces;

namespace QuoteAlgorithm
{
    public class CsvFileSourceProvider : ISourceProvider
    {
        private const string CsvFilePatten = @"(?:^|,)(?=[^""]|("")?)""?((?(1)[^""]*|[^,""]*))""?(?=,|$)";
        private const string LenderPattern = @"^[a-zA-Z\s]+$";
        private const string RatePattern = @"^\d+\.?\d*$";
        private const string AvailablePattern = @"^\d+$";
        private const int FieldsCount = 3;

        public IList<Offer> GetSource(string marketFilePath)
        {
            var existingOffers = new List<Offer>();
            try
            {
                if (File.Exists(marketFilePath))
                {
                    var lines = File.ReadAllLines(marketFilePath);
                    foreach (var line in lines.Skip(1))
                    {
                        var segments = Regex.Split(line, CsvFilePatten).Where(o => !string.IsNullOrWhiteSpace(o)).ToArray();
                        var lenderMatch = Regex.Match(segments[0], LenderPattern);
                        var rateMatch = Regex.Match(segments[1], RatePattern);
                        var availableMatch = Regex.Match(segments[2], AvailablePattern);
                        if (segments.Length == FieldsCount
                            && lenderMatch.Success
                            && rateMatch.Success
                            && availableMatch.Success)
                        {
                            var lender = lenderMatch.Value;
                            var rate = double.Parse(rateMatch.Value);
                            var available = long.Parse(availableMatch.Value);
                            existingOffers.Add(new Offer { Lender = lender, Rate = rate, Available = available });
                        }
                        else
                        {
                            throw new SourceFileFormatException("This market file format is invalid!");
                        }
                    }
                }
                else
                {
                    throw new IOException("File path doesn't exist.");
                }
            }
            catch (IOException ex)
            {
                throw new SourceFileIOException(ex.Message);
            }

            return existingOffers.OrderBy(o => o.Rate).ToList();
        }
    }
}
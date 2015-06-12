using System;
using QuoteAlgorithm;
using QuoteAlgorithm.Exceptions;
using QuoteAlgorithm.Interfaces;

namespace Quote
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                try
                {
                    var marketFilePath = args[0];
                    var loanAmount = args[1];
                    ISourceProvider sourceProvider = new CsvFileSourceProvider();
                    IQuoteProvider quoteProvider = new QuoteProvider(sourceProvider);
                    var quote = quoteProvider.CreateQuote(marketFilePath, loanAmount);
                    Console.WriteLine(quote);
                }
                catch (LoanAmountException ex)
                {
                    OutputErrorMessage(ex.Message);
                }
                catch (SourceFileFormatException ex)
                {
                    OutputErrorMessage("Please check the CSV source file format! " + ex.Message);
                }
                catch (SourceFileIOException ex)
                {
                    OutputErrorMessage(ex.Message);
                }
                catch (Exception ex)
                {
                    OutputErrorMessage("Someting went wrong! " + ex.Message);
                }
            }
            else
            {
                OutputErrorMessage("The application should take arguments in the form: \n" +
                                   "     cmd> [application] [market_file] [loan_amount] \n" +
                                   "Example: \n" +
                                   "     cmd> quote.exe market.csv 1500");
            }
        }

        private static void OutputErrorMessage(string errorMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(errorMessage);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
namespace QuoteAlgorithm.Entities
{
    public struct Offer
    {
        public string Lender { get; set; }
        public double Rate { get; set; }
        public long Available { get; set; }

        public override string ToString()
        {
            return string.Format(
                "Lender: {0}, Rate: {1:P3}, Available: {2}", 
                Lender, 
                Rate, 
                Available);
        }
    }
}
namespace QuoteAlgorithm.Entities
{
    public struct Quote
    {
        public long RequestAmount { get; set; }
        public double Rate { get; set; }
        public double MonthlyRepayment { get; set; }
        public double TotalRepayment { get; set; }

        public override string ToString()
        {
            return string.Format(
                "Requested amount: {0}\nRate: {1:P1}\nMonthly repayment: {2:C2}\nTotal repayment: {3:C2}",
                RequestAmount, 
                Rate, 
                MonthlyRepayment, 
                TotalRepayment);
        }
    }
}
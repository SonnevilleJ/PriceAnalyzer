using Sonneville.PriceTools;
using Sonneville.PriceTools.AutomatedTrading;

namespace SampleData.Internal
{
    internal class SamplePortfolio : ISamplePortfolio
    {
        public IPortfolio Portfolio
        {
            get { return new PortfolioFactory().ConstructPortfolio(TransactionHistory.Transactions); }
        }

        public string CsvString { get; set; }

        public ISecurityBasket TransactionHistory { get; set; }
    }
}
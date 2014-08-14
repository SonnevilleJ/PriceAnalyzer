using Sonneville.PriceTools.AutomatedTrading;
using Sonneville.PriceTools.AutomatedTrading.Implementation;

namespace Sonneville.PriceTools.SampleData.Internal
{
    public class SamplePortfolio
    {
        public IPortfolio Portfolio
        {
            get { return new PortfolioFactory().ConstructPortfolio(TransactionHistory.Transactions); }
        }

        public string CsvString { get; set; }

        public ISecurityBasket TransactionHistory { get; set; }
    }
}
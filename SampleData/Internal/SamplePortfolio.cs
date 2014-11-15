using Sonneville.PriceTools.AutomatedTrading;
using Sonneville.PriceTools.AutomatedTrading.Implementation;

namespace Sonneville.PriceTools.SampleData.Internal
{
    public class SamplePortfolio
    {
        private readonly IPortfolioFactory _portfolioFactory;

        public SamplePortfolio(IPortfolioFactory portfolioFactory)
        {
            _portfolioFactory = portfolioFactory;
        }

        public IPortfolio Portfolio
        {
            get { return _portfolioFactory.ConstructPortfolio(TransactionHistory.Transactions); }
        }

        public string CsvString { get; set; }

        public ISecurityBasket TransactionHistory { get; set; }
    }
}
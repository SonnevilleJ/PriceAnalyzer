using Sonneville.PriceTools;
using Sonneville.PriceTools.AutomatedTrading;
using Sonneville.PriceTools.Data.Csv;

namespace Test.Sonneville.PriceTools.PortfolioData.Internal
{
    internal class TestPortfolio : ITestPortfolio
    {
        public IPortfolio Portfolio
        {
            get { return new PortfolioFactory().ConstructPortfolio(TransactionHistory.Transactions); }
        }

        public string CsvString { get; set; }

        public ISecurityBasket TransactionHistory { get; set; }
    }
}
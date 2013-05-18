using Sonneville.PriceTools;
using Sonneville.PriceTools.AutomatedTrading;

namespace Test.Sonneville.PriceTools.PortfolioData
{
    public interface ITestPortfolio
    {
        IPortfolio Portfolio { get; }
        string CsvString { get; }
        ISecurityBasket TransactionHistory { get; }
    }
}
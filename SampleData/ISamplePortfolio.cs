using Sonneville.PriceTools;
using Sonneville.PriceTools.AutomatedTrading;

namespace SampleData
{
    public interface ISamplePortfolio
    {
        IPortfolio Portfolio { get; }
        string CsvString { get; }
        ISecurityBasket TransactionHistory { get; }
    }
}
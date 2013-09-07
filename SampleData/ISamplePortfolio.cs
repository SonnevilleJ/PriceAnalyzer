using Sonneville.PriceTools;
using Sonneville.PriceTools.AutomatedTrading.Implementation;

namespace SampleData
{
    public interface ISamplePortfolio
    {
        Portfolio Portfolio { get; }
        string CsvString { get; }
        ISecurityBasket TransactionHistory { get; }
    }
}
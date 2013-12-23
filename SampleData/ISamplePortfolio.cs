using Sonneville.PriceTools.AutomatedTrading.Implementation;

namespace Sonneville.PriceTools.SampleData
{
    public interface ISamplePortfolio
    {
        Portfolio Portfolio { get; }
        string CsvString { get; }
        ISecurityBasket TransactionHistory { get; }
    }
}
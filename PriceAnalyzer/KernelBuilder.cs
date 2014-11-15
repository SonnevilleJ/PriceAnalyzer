using Ninject;
using Sonneville.PriceTools.AutomatedTrading;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Fidelity;
using Sonneville.PriceTools.Google;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public static class KernelBuilder
    {
        public static IKernel Build()
        {
            return new StandardKernel(
                new PriceToolsModule(),
                new DataModule(),
                new FidelityModule(),
                new GoogleModule(),
                new AutomatedTradingModule());
        }
    }
}
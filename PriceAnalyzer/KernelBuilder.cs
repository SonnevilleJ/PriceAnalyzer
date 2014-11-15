using Ninject;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Fidelity;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public static class KernelBuilder
    {
        public static IKernel Build()
        {
            return new StandardKernel(new DataModule(), new FidelityModule());
        }
    }
}
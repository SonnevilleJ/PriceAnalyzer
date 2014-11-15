using Ninject.Modules;
using Sonneville.PriceTools.Data.Csv;

namespace Sonneville.PriceTools.Fidelity
{
    public class FidelityModule : NinjectModule
    {
        public override void Load()
        {
            Bind<TransactionHistoryCsvFile>().To<FidelityTransactionHistoryCsvFile>();
        }
    }
}
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using Sonneville.PriceTools.Data.Csv;

namespace Sonneville.PriceTools.Fidelity
{
    public class FidelityModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind(
                x => x.FromThisAssembly()
                    .SelectAllClasses()
                    .BindDefaultInterface()
                    .Configure(config => config.InSingletonScope()));

            Bind<TransactionHistoryCsvFile>().To<FidelityTransactionHistoryCsvFile>();
        }
    }
}
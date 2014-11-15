using Ninject.Modules;

namespace Sonneville.PriceTools.Data
{
    public class DataModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ITransactionFactory>().To<TransactionFactory>();
            Bind<IHoldingFactory>().To<HoldingFactory>();
            Bind<IPriceDataProvider>().To<PriceDataProvider>();
        }
    }
}
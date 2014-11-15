using Ninject.Modules;
using Sonneville.PriceTools.Data;

namespace Sonneville.PriceTools.Google
{
    public class GoogleModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPriceHistoryQueryUrlBuilder>().To<GooglePriceHistoryQueryUrlBuilder>();
            Bind<IPriceHistoryCsvFileFactory>().To<GooglePriceHistoryCsvFileFactory>();
        }
    }
}
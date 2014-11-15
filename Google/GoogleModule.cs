using Ninject.Extensions.Conventions;
using Ninject.Modules;
using Sonneville.PriceTools.Data;

namespace Sonneville.PriceTools.Google
{
    public class GoogleModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind(
                x => x.FromThisAssembly()
                    .SelectAllClasses()
                    .BindDefaultInterface()
                    .Configure(config => config.InSingletonScope()));

            Bind<IPriceHistoryQueryUrlBuilder>().To<GooglePriceHistoryQueryUrlBuilder>();
            Bind<IPriceHistoryCsvFileFactory>().To<GooglePriceHistoryCsvFileFactory>();
        }
    }
}
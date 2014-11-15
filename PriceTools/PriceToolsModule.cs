using Ninject.Modules;

namespace Sonneville.PriceTools
{
    public class PriceToolsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPriceSeriesFactory>().To<PriceSeriesFactory>();
            Bind<IWebClient>().To<WebClientWrapper>();
        }
    }
}
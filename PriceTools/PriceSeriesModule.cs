using Ninject.Modules;

namespace Sonneville.PriceTools
{
    public class PriceSeriesModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPriceSeriesFactory>().To<PriceSeriesFactory>();
        }
    }
}
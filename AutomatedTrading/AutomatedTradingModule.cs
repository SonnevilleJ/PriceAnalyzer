using Ninject.Modules;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public class AutomatedTradingModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IBrokerage>().To<SimulatedBrokerage>();
        }
    }
}
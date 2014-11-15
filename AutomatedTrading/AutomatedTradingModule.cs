using Ninject.Modules;
using Ninject.Extensions.Conventions;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public class AutomatedTradingModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind(
                x => x.FromThisAssembly()
                    .SelectAllClasses()
                    .BindDefaultInterface()
                    .Configure(config => config.InSingletonScope()));

            Bind<IBrokerage>().To<SimulatedBrokerage>();
        }
    }
}
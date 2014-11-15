using Ninject.Modules;
using Ninject.Extensions.Conventions;

namespace Sonneville.PriceTools
{
    public class PriceToolsModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind(
                x => x.FromThisAssembly()
                    .SelectAllClasses()
                    .BindDefaultInterface()
                    .Configure(config => config.InSingletonScope()));

            Bind<IWebClient>().To<WebClientWrapper>();
        }
    }
}
using Ninject.Modules;
using Ninject.Extensions.Conventions;

namespace Sonneville.PriceTools.Data
{
    public class DataModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind(
                x => x.FromThisAssembly()
                    .SelectAllClasses()
                    .BindDefaultInterface()
                    .Configure(config => config.InSingletonScope()));
        }
    }
}
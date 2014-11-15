using Ninject.Modules;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public class AutomatedTradingModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IBrokerage>().To<SimulatedBrokerage>();
            Bind<ITradingProcess>().To<TradingProcess>();
            Bind<IAnalysisEngine>().To<AnalysisEngine>();
            Bind<ISecurityBasketCalculator>().To<SecurityBasketCalculator>();
            Bind<IPortfolioFactory>().To<PortfolioFactory>();
        }
    }
}
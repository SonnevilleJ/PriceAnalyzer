using System;
using System.Collections.Generic;
using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public interface IPortfolioFactory
    {
        IPortfolio ConstructPortfolio(params ITransaction[] transactions);

        IPortfolio ConstructPortfolio(IEnumerable<ITransaction> transactions);

        IPortfolio ConstructPortfolio(string ticker, params ITransaction[] transactions);

        IPortfolio ConstructPortfolio(string ticker, IEnumerable<ITransaction> transactions);

        IPortfolio ConstructPortfolio(DateTime dateTime, decimal openingDeposit, params ITransaction[] transactions);

        IPortfolio ConstructPortfolio(DateTime dateTime, decimal openingDeposit, IEnumerable<ITransaction> transactions);

        IPortfolio ConstructPortfolio(string ticker, DateTime dateTime, decimal openingDeposit, params ITransaction[] transactions);

        IPortfolio ConstructPortfolio(string ticker, DateTime dateTime, decimal openingDeposit, IEnumerable<ITransaction> transactions);

        IPriceSeries ConstructPriceSeries(IPortfolio portfolio, IPriceDataProvider priceDataProvider);
    }
}
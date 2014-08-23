﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public class TradingEngine
    {
        private readonly ISecurityBasketCalculator _securityBasketCalculator;
        private readonly IPortfolio _portfolio;

        public TradingEngine(ISecurityBasketCalculator securityBasketCalculator, IPortfolio portfolio)
        {
            _securityBasketCalculator = securityBasketCalculator;
            _portfolio = portfolio;
        }

        public IEnumerable<Order> DetermineOrdersFor(IPriceSeries stockBeingConsidered, DateTime startDate, DateTime endDate, IList<Order> openOrders)
        {
            var sharesToBuy = CalculateSharesToBuy(stockBeingConsidered, startDate, endDate, openOrders);
            if (HaveAvailableFunds(stockBeingConsidered, endDate) && sharesToBuy > 0)
            {
                yield return CreateOrder(OrderType.Buy, stockBeingConsidered.Ticker, sharesToBuy, endDate);
            }
            var sharesToSell = CalculateSharesToSell(stockBeingConsidered, startDate, endDate, openOrders);
            if (sharesToSell > 0)
            {
                yield return CreateOrder(OrderType.Sell, stockBeingConsidered.Ticker, sharesToSell, endDate);
            }
        }

        private bool HaveAvailableFunds(IPriceSeries stockBeingConsidered, DateTime endDate)
        {
            return _portfolio.GetAvailableCash(endDate) >= stockBeingConsidered[endDate];
        }

        private decimal CalculateSharesToBuy(IPriceSeries stockBeingConsidered, DateTime startDate, DateTime endDate, IList<Order> openOrders)
        {
            var shouldBuy = stockBeingConsidered[endDate] > stockBeingConsidered[startDate];
            if (shouldBuy)
            {
                return 1;
            }
            return 0;
        }

        private decimal CalculateSharesToSell(IPriceSeries stockBeingConsidered, DateTime startDate, DateTime endDate, IList<Order> openOrders)
        {
            var shouldSell = stockBeingConsidered[startDate] > stockBeingConsidered[endDate];
            if (shouldSell)
            {
                var position = _portfolio.GetPosition(stockBeingConsidered.Ticker);
                if (position != null)
                {
                    var shareTransactions = position.Transactions.Cast<IShareTransaction>();
                    var shares = _securityBasketCalculator.GetHeldShares(shareTransactions, endDate);
                    var sharesWaitingToBeSold = CalculateSharesWaitingToBeSold(openOrders, stockBeingConsidered.Ticker);
                    return shares - sharesWaitingToBeSold;
                }
            }
            return 0;
        }

        private decimal CalculateSharesWaitingToBeSold(IEnumerable<Order> openOrders, string ticker)
        {
            return openOrders.Where(order=>order.Ticker == ticker).Select(GetChangeInShares).Sum();
        }

        private decimal GetChangeInShares(Order order)
        {
            switch (order.OrderType)
            {
                case OrderType.Buy:
                    return -order.Shares;
                case OrderType.Sell:
                    return order.Shares;
                default:
                    return 0;
            }
        }

        private static Order CreateOrder(OrderType orderType, string ticker, decimal shares, DateTime issueDate)
        {
            return new Order
            {
                OrderType = orderType,
                Shares = shares,
                Issued = issueDate,
                Ticker = ticker
            };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sonneville.PriceTools.AutomatedTrading
{
    ///// <summary>
    ///// A trading account which communicates with a brokerage to perform execution of orders.
    ///// </summary>
    //public abstract class AsynchronousTradingAccount : TradingAccount
    //{
    //    #region Private Members

    //    private readonly IList<Tuple<Order, Task, CancellationTokenSource>> _items = new List<Tuple<Order, Task, CancellationTokenSource>>();

    //    #endregion

    //    #region Constructors

    //    protected AsynchronousTradingAccount(TradingAccountFeatures features)
    //        : base(features)
    //    {
    //    }

    //    #endregion

    //    #region Public Interface

    //    /// <summary>
    //    /// Submits an order for execution by the brokerage.
    //    /// </summary>
    //    /// <param name="order">The <see cref="Order"/> to execute.</param>
    //    public override void Submit(Order order)
    //    {
    //        if (!ValidateOrder(order)) throw new ArgumentOutOfRangeException("order", order, Strings.TradingAccount_Submit_Cannot_execute_this_order_);

    //        var cts = new CancellationTokenSource();
    //        var token = cts.Token;
    //        var task = new Task(() => ProcessOrder(order, token), TaskCreationOptions.PreferFairness);
    //        lock (_items) _items.Add(new Tuple<Order, Task, CancellationTokenSource>(order, task, cts));
    //        task.Start();
    //    }

    //    /// <summary>
    //    /// Attempts to cancel an <see cref="Order"/> before it is filled.
    //    /// </summary>
    //    /// <param name="order">The <see cref="Order"/> to attempt to cancel.</param>
    //    public override void TryCancelOrder(Order order)
    //    {
    //        Tuple<Order, Task, CancellationTokenSource> value;
    //        lock (_items) value = _items.First(tuple => tuple.Item1 == order);
    //        var cts = value.Item3;

    //        cts.Cancel();
    //    }

    //    /// <summary>
    //    /// Blocks the calling thread until all submitted orders are filled, cancelled, or expired.
    //    /// </summary>
    //    public override void WaitAll()
    //    {
    //        do
    //        {
    //            int count;
    //            lock (_items) count = _items.Count;
    //            if (count == 0) break;
    //            Thread.Sleep(5);
    //        } while (true);
    //    }

    //    #endregion
    //}
}

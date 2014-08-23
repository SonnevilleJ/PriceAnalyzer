using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sonneville.Utilities;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public class SimulatedBroker
    {
        public OrderStatus SubmitOrder(Order order)
        {
            
            var _orderStatus = new OrderStatus();
            _orderStatus.Ticker = order.Ticker;
            _orderStatus.Id = Guid.NewGuid();
            _orderStatus.Price = order.Price;
            _orderStatus.Shares = order.Shares;
            _orderStatus.OrderType = order.OrderType;
            _orderStatus.SubmitTime = Clock.Now;
            return _orderStatus;
        }
    }
}

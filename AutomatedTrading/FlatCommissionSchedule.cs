namespace Sonneville.PriceTools.AutomatedTrading
{
    public class FlatCommissionSchedule : ICommissionSchedule
    {
        private readonly decimal _price;

        public FlatCommissionSchedule(decimal price)
        {
            _price = price;
        }

        public virtual decimal PriceCheck(Order order)
        {
            return _price;
        }
    }
}

using System;
using System.Globalization;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    public class TransactionFactory : ITransactionFactory
    {
        private readonly Guid _factoryGuid;

        public TransactionFactory()
            :this(Guid.Parse("26491456-7E65-421B-B4C3-984527311CED"))
        {
        }

        public TransactionFactory(Guid factoryGuid)
        {
            _factoryGuid = factoryGuid;
        }

        public Deposit ConstructDeposit(DateTime settlementDate, decimal amount)
        {
            return new Deposit(_factoryGuid, settlementDate, amount);
        }

        public Withdrawal ConstructWithdrawal(DateTime settlementDate, decimal amount)
        {
            return new Withdrawal(_factoryGuid, settlementDate, amount);
        }

        public DividendReceipt ConstructDividendReceipt(DateTime settlementDate, decimal amount)
        {
            return new DividendReceipt(_factoryGuid, settlementDate, amount);
        }

        public CashTransaction ConstructCashTransaction(OrderType transactionType, DateTime settlementDate, decimal amount)
        {
            return (CashTransaction)ConstructTransaction(transactionType, settlementDate, String.Empty, amount, 0, 0);
        }

        public DividendReinvestment ConstructDividendReinvestment(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission = 0)
        {
            return new DividendReinvestment(_factoryGuid, ticker, settlementDate, shares, price, commission);
        }

        public Buy ConstructBuy(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission = 0.00m)
        {
            return new Buy(_factoryGuid, ticker, settlementDate, shares, price, commission);
        }

        public Sell ConstructSell(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission = 0.00m)
        {
            return new Sell(_factoryGuid, ticker, settlementDate, shares, price, commission);
        }

        public BuyToCover ConstructBuyToCover(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission = 0.00m)
        {
            return new BuyToCover(_factoryGuid, ticker, settlementDate, shares, price, commission);
        }

        public SellShort ConstructSellShort(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission = 0.00m)
        {
            return new SellShort(_factoryGuid, ticker, settlementDate, shares, price, commission);
        }

        public ShareTransaction ConstructShareTransaction(OrderType type, string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission)
        {
            return (ShareTransaction) ConstructTransaction(type, settlementDate, ticker, price, shares, commission);
        }

        public Transaction ConstructTransaction(OrderType type, DateTime date, string ticker, decimal price, decimal shares, decimal commission)
        {
            switch (type)
            {
                case OrderType.Deposit:
                    return ConstructDeposit(date, price);
                case OrderType.Withdrawal:
                    return ConstructWithdrawal(date, price);
                case OrderType.Buy:
                    return ConstructBuy(ticker, date, shares, price, commission);
                case OrderType.BuyToCover:
                    return ConstructBuyToCover(ticker, date, shares, price, commission);
                case OrderType.DividendReceipt:
                    return ConstructDividendReceipt(date, price);
                case OrderType.DividendReinvestment:
                    return ConstructDividendReinvestment(ticker, date, shares, price, commission);
                case OrderType.Sell:
                    return ConstructSell(ticker, date, shares, price, commission);
                case OrderType.SellShort:
                    return ConstructSellShort(ticker, date, shares, price, commission);
                default:
                    throw new ArgumentOutOfRangeException("type", String.Format(CultureInfo.CurrentCulture, Strings.TransactionFactory_CreateTransaction_Unknown_OrderType___0_, type));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sonneville.PriceTools
{
    [Serializable]
    public class Transaction : ITransaction
    {
        private readonly DateTime _date;
        private readonly TransactionType _type;
        private readonly string _ticker;
        private readonly double _shares;
        private readonly decimal _price;
        private readonly decimal _commission;

        public Transaction(DateTime date, TransactionType type, string ticker, decimal price, double shares = 1, decimal commission = 0)
        {
            _date = date;
            _type = type;
            _ticker = ticker;
            _shares = shares;
            _price = price;
            _commission = commission;
        }

        public DateTime Date
        {
            get { return _date; }
        }

        public TransactionType TransactionType
        {
            get { return _type; }
        }

        public string Ticker
        {
            get { return _ticker; }
        }

        public double Shares
        {
            get { return _shares; }
        }

        public decimal Price
        {
            get { return _price; }
        }

        public decimal Commission
        {
            get { return _commission; }
        }
    }
}

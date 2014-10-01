using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public class PositionSummaryViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TransactionSummary> _transactionSummaries;

        public PositionSummaryViewModel()
        {
            TransactionSummaries = new ObservableCollection<TransactionSummary>
            {
                new TransactionSummary
                {
                    Ticker = "DE",
                    BoughtPrice = 1.00,
                    SoldPrice = 2.00,
                    CurrentPrice = 2.00,
                    Volume = 10.0,
                    NetChange = 10.00
                },
                new TransactionSummary
                {
                    Ticker = "IBM",
                    BoughtPrice = 10.00,
                    SoldPrice = 20.00,
                    CurrentPrice = 20.00,
                    Volume = 10.0,
                    NetChange = 100.00
                },
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string PortfolioValue
        {
            get { return "500"; }
        }

        public string NetChange
        {
            get { return "500%"; }
        }

        public ObservableCollection<TransactionSummary> TransactionSummaries
        {
            get { return _transactionSummaries; }
            set
            {
                _transactionSummaries = value;
                OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
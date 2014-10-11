using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public class PositionSummaryViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TransactionSummary> _transactionSummaries;

        public PositionSummaryViewModel()
        {
            TransactionSummaries = new ObservableCollection<TransactionSummary>();

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

        public void UpdateTransactions(IEnumerable<IShareTransaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                var summary = new TransactionSummary();
                summary.Ticker = transaction.Ticker;
                summary.BoughtPrice = (double) transaction.Price;
                summary.Volume = (double) transaction.Shares;

                TransactionSummaries.Add(summary);
            }
        }
    }
}
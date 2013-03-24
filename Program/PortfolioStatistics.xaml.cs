using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using Sonneville.PriceTools;
using Sonneville.PriceTools.AutomatedTrading;
using Sonneville.PriceTools.Fidelity;
using Statistics;

namespace Program
{
    /// <summary>
    /// Interaction logic for PortfolioStatistics.xaml
    /// </summary>
    public partial class PortfolioStatistics
    {
        private ObservableCollection<IHolding> _collection = new ObservableCollection<IHolding>();
        private static readonly IPortfolioFactory _portfolioFactory;

        public PortfolioStatistics()
        {
            InitializeComponent();
            dataGrid.ItemsSource = _collection;
        }

        static PortfolioStatistics()
        {
            _portfolioFactory = new PortfolioFactory();
        }

        private void BrowseClick(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog {Filter = "CSV Files (*.csv)|*.csv"};

            if (ofd.ShowDialog().Value)
            {
                var path = ofd.FileName;
                csvPath.Text = path;
            }
        }

        private void CalculateClick(object sender, RoutedEventArgs e)
        {
            var path = csvPath.Text;
            var ticker = cashTicker.Text;
            if (!File.Exists(path))
            {
                MessageBox.Show("Must specify path to CSV file containing portfolio data.");
                return;
            }
            var portfolio = ImportPortfolio(path, ticker);

            CalculateStatistics(portfolio);
            PopulateHoldings(portfolio.CalculateHoldings(DateTime.Now));
        }

        private void PopulateHoldings(IEnumerable<IHolding> holdings)
        {
            _collection = new ObservableCollection<IHolding>(holdings);
            dataGrid.UpdateLayout();
        }

        private void CalculateStatistics(ISecurityBasket portfolio)
        {
            var date = portfolio.Tail;
            grossProfit.Text = portfolio.CalculateGrossProfit(date).ToString("C");
            netProfit.Text = portfolio.CalculateNetProfit(date).ToString("C");
            meanGrossProfit.Text = portfolio.CalculateAverageProfit(date).ToString("C");
            var grossReturn = portfolio.CalculateAnnualGrossReturn(date);
            if(grossReturn.HasValue) annualGrossReturn.Text = grossReturn.Value.ToString("P");
            standardDeviation.Text = portfolio.CalculateStandardDeviation(date).ToString("C");
            tDistribution.Text = portfolio.CalculateHoldings(date).Select(h => h.GrossProfit()).StudentTDistribution().ToString("P");
        }

        private static IPortfolio ImportPortfolio(string path, string ticker)
        {
            using (var reader = File.Open(path, FileMode.Open))
            {
                var data = new FidelityTransactionHistoryCsvFile(reader);
                return _portfolioFactory.ConstructPortfolio(ticker, data.Transactions);
            }
        }
    }
}

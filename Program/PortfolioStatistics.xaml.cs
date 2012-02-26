﻿using System.IO;
using System.Windows;
using Microsoft.Win32;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Fidelity;

namespace Program
{
    /// <summary>
    /// Interaction logic for PortfolioStatistics.xaml
    /// </summary>
    public partial class PortfolioStatistics
    {
        public PortfolioStatistics()
        {
            InitializeComponent();
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

            var date = portfolio.Tail;
            grossProfit.Text = portfolio.CalculateGrossProfit(date).ToString("C");
            netProfit.Text = portfolio.CalculateNetProfit(date).ToString("C");
            var grossReturn = portfolio.CalculateAnnualGrossReturn(date);
            if(grossReturn.HasValue) annualGrossReturn.Text = grossReturn.Value.ToString("P");
            var netReturn = portfolio.CalculateAnnualNetReturn(date);
            if(netReturn.HasValue) annualNetReturn.Text = netReturn.Value.ToString("P");
            meanGrossProfit.Text = portfolio.CalculateAverageProfit(date).ToString("C");
            medianNetProfit.Text = portfolio.CalculateMedianProfit(date).ToString("C");
        }

        private static Portfolio ImportPortfolio(string path, string ticker)
        {
            using (var reader = File.Open(path, FileMode.Open))
            {
                var data = new FidelityBrokerageLinkTransactionHistoryCsvFile(reader);
                return PortfolioFactory.ConstructPortfolio(data, ticker);
            }
        }
    }
}

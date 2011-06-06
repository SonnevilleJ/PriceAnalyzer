using System;
using System.Windows;
using Sonneville.PriceTools;

namespace Sonneville.PriceAnalyzer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            PriceSeries priceSeries = PriceSeriesFactory.CreatePriceSeries("DE");
            priceSeries.DownloadPriceData(new DateTime(2011, 1, 1));

            chart1.PriceSeries = priceSeries;
            chart1.LastDisplayedPeriod = priceSeries.Tail;
            chart1.FirstDisplayedPeriod = priceSeries.Head;
        }
    }
}

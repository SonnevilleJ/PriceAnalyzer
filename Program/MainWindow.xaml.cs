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
        readonly PriceSeries _priceSeries = PriceSeriesFactory.CreatePriceSeries("DE");
        
        public MainWindow()
        {
            InitializeComponent();
            
            _priceSeries.DownloadPriceData(new DateTime(2011, 1, 1));
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            DrawChart();
        }

        private void DrawChart()
        {
            chart1.PriceSeries = _priceSeries;
        }
    }
}

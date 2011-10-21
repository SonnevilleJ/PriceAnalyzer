using System;
using System.Windows;
using Sonneville.PriceTools;

namespace Program
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

            ContentRendered += DrawPriceChart;
        }

        void DrawPriceChart(object sender, EventArgs e)
        {
            _priceSeries.DownloadPriceData(new DateTime(2011, 1, 1));
            DrawChart();

            ContentRendered -= DrawPriceChart;
        }

        private void DrawChart()
        {
            chart1.PriceSeries = _priceSeries;
        }
    }
}

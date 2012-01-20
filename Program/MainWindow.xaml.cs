using System;
using Sonneville.PriceTools;
using Sonneville.PriceTools.SamplePriceData;

namespace Program
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly PriceSeries _priceSeries = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
        
        public MainWindow()
        {
            InitializeComponent();

            ContentRendered += DrawPriceChart;
        }

        private void DrawPriceChart(object sender, EventArgs e)
        {
            DrawChart();

            ContentRendered -= DrawPriceChart;
        }

        private void DrawChart()
        {
            chartCanvas.PriceSeries = _priceSeries;
        }
    }
}

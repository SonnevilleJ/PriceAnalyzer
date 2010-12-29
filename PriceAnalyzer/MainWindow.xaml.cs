using System.Windows;

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

            //IPriceSeries series = new PriceSeries(PriceSeriesResolution.Days, null);
            //ChartBuilder chartist = new ChartBuilder(series);
            //background.Source = chartist.GenerateCandlestickChart();
        }
    }
}

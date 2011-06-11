using System.Windows;
using System.Windows.Media;
using Sonneville.PriceTools;

namespace Sonneville.PriceChartTools
{
    /// <summary>
    /// Interaction logic for AutosizableChart.xaml
    /// </summary>
    public abstract partial class AutosizableChart
    {
        #region Private Members

        private IPriceSeries _priceSeries;

        #endregion

        #region Constructors

        protected AutosizableChart()
            : this(false)
        {
        }

        protected AutosizableChart(bool connectPeriods)
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        private double ScaleFactor
        {
            get { return ActualHeight / 300.0; }
        }

        private void SizeChangedOverride(object sender, SizeChangedEventArgs e)
        {
            var factor = ScaleFactor;
            scaleTransform.ScaleX = factor;
            scaleTransform.ScaleY = factor;
            chartCanvas.Width = ActualWidth - chartBorder.Margin.Right - chartBorder.Margin.Left;
            chartCanvas.Height = ActualHeight - chartBorder.Margin.Bottom - chartBorder.Margin.Top;
        }

        #endregion

        public abstract PointCollection GetPolylinePoints(double left, double center, double right, double? open, double? high, double? low, double close);
    }
}

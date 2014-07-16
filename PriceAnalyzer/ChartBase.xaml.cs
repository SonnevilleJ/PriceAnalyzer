using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    /// <summary>
    /// Interaction logic for ChartBase.xaml
    /// </summary>
    public abstract partial class ChartBase : UserControl
    {
        protected ChartBase()
        {
            InitializeComponent();
            SizeChanged += delegate { if (PricePeriods != null) DrawChart(); };
        }

        public IList<IPricePeriod> PricePeriods { get; private set; }

        public void DrawPricePeriods(IList<IPricePeriod> pricePeriods)
        {
            PricePeriods = pricePeriods;
            Horizontal.PricePeriods = pricePeriods;
            DrawChart();
        }

        private void DrawChart()
        {
            if (Equals0(_canvas.ActualHeight) || Equals0(_canvas.ActualWidth))
            {
                return;
            }

            var highestHigh = (double) PricePeriods.Max(pricePeriod => pricePeriod.High);
            var lowestLow = (double) PricePeriods.Min(pricePeriod => pricePeriod.Low);

            var dollarRange = (highestHigh - lowestLow);
            var pixelsPerDollar = (_canvas.ActualHeight/dollarRange);
            Vertical.DrawVerticalAxis(highestHigh, lowestLow, pixelsPerDollar);

            var pixelsPerDay = _canvas.ActualWidth/PricePeriods.Count;
            Horizontal.DrawHorizontalAxis(pixelsPerDay);
            
            DrawCanvas(highestHigh, lowestLow, pixelsPerDollar, pixelsPerDay);
        }

        private bool Equals0(double value)
        {
            return Math.Abs(value) < 0.1;
        }

        protected static Brush CloseColorBrush(decimal priorPeriodClose, decimal currentPeriodClose)
        {
            if (priorPeriodClose < currentPeriodClose)
            {
                return Brushes.Black;
            }
            else
            {
                return Brushes.Firebrick;
            }
        }

        protected static Line CreateLine(double x1, double y1, double x2, double y2, Brush stroke)
        {
            return new Line {X1 = x1, Y1 = y1, X2 = x2, Y2 = y2, Stroke = stroke};
        }

        protected abstract void DrawCanvas(double maxYdollar, double minYdollar, double pixelsPerDollar, double pixelsPerDay);
    }
}
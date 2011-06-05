using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Sonneville.PriceTools;

namespace Sonneville.PriceChartTools
{
    /// <summary>
    /// Interaction logic for CandlestickChart.xaml
    /// </summary>
    public partial class CandlestickChart : UserControl
    {
        private IPriceSeries _priceSeries;

        /// <summary>
        /// Gets or sets the thickness of the stroke used to draw the chart.
        /// </summary>
        public int StrokeThickness { get; set; }

        public CandlestickChart()
        {
            InitializeComponent();

            StrokeThickness = 1;
            CandleWidth = 6;
            CandleSpacing = 1;
            BufferRight = 1;
            BufferTop = 5;
            BufferBottom = 10;

            PriceSeries = PriceSeriesFactory.CreatePriceSeries("DE");
            PriceSeries.DownloadPriceData(new DateTime(2011, 4, 1));
        }

        /// <summary>
        /// Gets or sets the <see cref="IPriceSeries"/> containing the price data to be charted.
        /// </summary>
        public IPriceSeries PriceSeries
        {
            get { return _priceSeries; }
            set
            {
                _priceSeries = value;
                DrawChart();
            }
        }

        /// <summary>
        /// Gets or sets the space in candle widths that should be placed between candles.
        /// </summary>
        private double CandleSpacing { get; set; }

        /// <summary>
        /// Gets or sets the space in candle widths between the last candle and the edge of the chart.
        /// </summary>
        private double BufferRight { get; set; }

        private double BufferLeft { get; set; }

        private double BufferTop { get; set; }

        private double BufferBottom { get; set; }

        /// <summary>
        /// Gets or sets the horizontal space taken up by half a candle.
        /// </summary>
        private int HalfCandleWidth { get; set; }

        /// <summary>
        /// Gets or sets the horizontal space taken up by a full candle.
        /// </summary>
        private int CandleWidth
        {
            get { return HalfCandleWidth*2; }
            set { HalfCandleWidth = value/2; }
        }

        private void ChartGridSizeChanged(object sender, EventArgs e)
        {
            chartCanvas.Width = chartGrid.ActualWidth
                                - chartBorder.Margin.Left
                                - chartBorder.Margin.Right
                                - chartBorder.BorderThickness.Left
                                - chartBorder.BorderThickness.Right;
            chartCanvas.Height = chartGrid.ActualHeight
                                 - chartBorder.Margin.Top
                                 - chartBorder.Margin.Bottom
                                 - chartBorder.BorderThickness.Top
                                 - chartBorder.BorderThickness.Bottom;
            DrawChart();
        }

        private void DrawChart()
        {
            chartCanvas.Children.Clear();

            var orderedPeriods = PriceSeries.PricePeriods.OrderByDescending(period =>period.Head);
            for (int i = 0; i < orderedPeriods.Count(); i++)
            {
                double open = Convert.ToDouble(orderedPeriods.ElementAt(i).Open.Value);
                double high = Convert.ToDouble(orderedPeriods.ElementAt(i).High.Value);
                double low = Convert.ToDouble(orderedPeriods.ElementAt(i).Low.Value);
                double close = Convert.ToDouble(orderedPeriods.ElementAt(i).Close);
                double lastClose;
                try
                {
                    var previous = orderedPeriods.ElementAt(i + 1);
                    lastClose = Convert.ToDouble(previous.Close);
                }
                catch (ArgumentOutOfRangeException)
                {
                    lastClose = 0;
                }

                DrawPeriod(i, open, high, low, close, lastClose);
            }
        }

        private void DrawPeriod(double candlestick, double open, double high, double low, double close, double lastClose)
        {
            var candle = new Polyline();
            GetPoints(candle, XNormalize(candlestick), YNormalize(open), YNormalize(high), YNormalize(low), YNormalize(close));

            candle.Stroke = close >= lastClose
                                ? Brushes.Black
                                : Brushes.Red;
            candle.Fill = close >= open
                              ? Brushes.White
                              : candle.Stroke;
            candle.StrokeThickness = StrokeThickness;
            chartCanvas.Children.Add(candle);
        }

        private void GetPoints(Polyline candle, double center, double open, double high, double low, double close)
        {
            var left = center - HalfCandleWidth;
            var right = center + HalfCandleWidth;

            candle.Points.Add(new Point(center, low));
            candle.Points.Add(new Point(center, open));
            candle.Points.Add(new Point(left, open));
            candle.Points.Add(new Point(left, close));
            candle.Points.Add(new Point(center, close));
            candle.Points.Add(new Point(center, high));
            candle.Points.Add(new Point(center, close));
            candle.Points.Add(new Point(right, close));
            candle.Points.Add(new Point(right, open));
            candle.Points.Add(new Point(center, open));
            candle.Points.Add(new Point(center, low));
        }

        private double XNormalize(double candlePosition)
        {
            var bufferRight = (BufferRight * CandleWidth) + HalfCandleWidth;
            var spacing = CandleSpacing * CandleWidth;
            var offset = candlePosition*(CandleWidth + spacing) + HalfCandleWidth;
            var result = chartCanvas.Width - offset - bufferRight;
            return result;
        }

        private DateTime FirstDisplayedPeriod
        {
            get { return new DateTime(2011, 4, 1); }
        }

        private DateTime LastDisplayedPeriod
        {
            get { return DateTime.Now; }
        }

        private double MinDisplayedPrice
        {
            get
            {
                return (double) PriceSeries.PricePeriods.Where(p => p.Head >= FirstDisplayedPeriod && p.Tail <= LastDisplayedPeriod).Min(p => p.Low.Value);
            }
        }

        private double MaxDisplayedPrice
        {
            get
            {
                return (double) PriceSeries.PricePeriods.Where(p => p.Head >= FirstDisplayedPeriod && p.Tail <= LastDisplayedPeriod).Max(p => p.High.Value);
            }
        }

        private double YNormalize(double price)
        {
            var minimum = MinDisplayedPrice - BufferBottom;
            var maximum = MaxDisplayedPrice + BufferTop;
            var position = (price - minimum)/(maximum - minimum);
            var flippedPosition = chartCanvas.Height - (position * chartCanvas.Height);
            return flippedPosition;
        }
    }
}

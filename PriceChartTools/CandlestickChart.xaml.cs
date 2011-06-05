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

        public CandlestickChart()
        {
            InitializeComponent();

            YMax = 100;
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

        private double YMax { get; set; }

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

                DrawCandlestick(i, low, open, close, high, lastClose);
            }
        }

        private void DrawCandlestick(double candleCenter, double low, double open, double close, double high, double lastClose)
        {
            var candle = new Polyline();
            var normalizedOpen = YNormalize(open);
            var normalizedHigh = YNormalize(high);
            var normalizedLow = YNormalize(low);
            var normalizedClose = YNormalize(close);
            var normalizedCenter = XNormalize(candleCenter);
            var candleLeft = normalizedCenter - HalfCandleWidth;
            var candleRight = normalizedCenter + HalfCandleWidth;

            candle.Points.Add(new Point(normalizedCenter, normalizedLow));
            candle.Points.Add(new Point(normalizedCenter, normalizedOpen));
            candle.Points.Add(new Point(candleLeft, normalizedOpen));
            candle.Points.Add(new Point(candleLeft, normalizedClose));
            candle.Points.Add(new Point(normalizedCenter, normalizedClose));
            candle.Points.Add(new Point(normalizedCenter, normalizedHigh));
            candle.Points.Add(new Point(normalizedCenter, normalizedClose));
            candle.Points.Add(new Point(candleRight, normalizedClose));
            candle.Points.Add(new Point(candleRight, normalizedOpen));
            candle.Points.Add(new Point(normalizedCenter, normalizedOpen));
            candle.Points.Add(new Point(normalizedCenter, normalizedLow));

            candle.Stroke = close >= lastClose
                                ? Brushes.Black
                                : Brushes.Red;
            candle.Fill = close >= open
                              ? Brushes.White
                              : candle.Stroke;
            candle.StrokeThickness = 1;
            chartCanvas.Children.Add(candle);
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
                DateTime head = FirstDisplayedPeriod;
                DateTime tail = LastDisplayedPeriod;

                return (double) PriceSeries.PricePeriods.Where(p => p.Head >= head && p.Tail <= tail).Min(p => p.Low.Value);
            }
        }

        private double MaxDisplayedPrice
        {
            get
            {
                DateTime head = FirstDisplayedPeriod;
                DateTime tail = LastDisplayedPeriod;

                return (double) PriceSeries.PricePeriods.Where(p => p.Head >= head && p.Tail <= tail).Max(p => p.High.Value);
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

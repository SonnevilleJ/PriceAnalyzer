using System;
using System.Linq;
using System.Windows.Media;
using System.Windows.Shapes;
using Sonneville.PriceTools;

namespace Sonneville.PriceChartTools
{
    /// <summary>
    /// Interaction logic for Chart.xaml
    /// </summary>
    public abstract partial class Chart : IChart
    {
        private IPriceSeries _priceSeries;

        protected Chart()
        {
            InitializeComponent();

            StrokeThickness = 1;
            CandleWidth = 6;
            CandleSpacing = 1;
            BufferRight = 1;
            BufferTop = 5;
            BufferBottom = 10;

            PriceSeries priceSeries = PriceSeriesFactory.CreatePriceSeries("DE");
            priceSeries.DownloadPriceData(new DateTime(2011, 4, 1));
            FirstDisplayedPeriod = priceSeries.Head;
            LastDisplayedPeriod = priceSeries.Tail;

            PriceSeries = priceSeries;
        }

        /// <summary>
        /// Gets or sets the thickness of the stroke used to draw the chart.
        /// </summary>
        public int StrokeThickness { get; set; }

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
        public double CandleSpacing { get; set; }

        /// <summary>
        /// Gets or sets the space in candle widths between the last candle and the right edge of the chart.
        /// </summary>
        public double BufferRight { get; set; }

        /// <summary>
        /// Gets or sets the space in candle widths between the first candle and the left edge of the chart.
        /// </summary>
        public double BufferLeft { get; set; }

        /// <summary>
        /// Gets or sets the space in price units between the <see cref="MaxDisplayedPrice"/> and the top edge of the chart.
        /// </summary>
        public double BufferTop { get; set; }

        /// <summary>
        /// Gets or sets the space in price units between the <see cref="MinDisplayedPrice"/> and the bottom edge of the chart.
        /// </summary>
        public double BufferBottom { get; set; }

        /// <summary>
        /// Gets or sets the horizontal space taken up by half a candle.
        /// </summary>
        public int HalfCandleWidth { get; set; }

        /// <summary>
        /// Gets or sets the horizontal space taken up by a full candle.
        /// </summary>
        public int CandleWidth
        {
            get { return HalfCandleWidth*2; }
            set { HalfCandleWidth = value/2; }
        }

        /// <summary>
        /// Gets or sets the first period displayed on the chart.
        /// </summary>
        public DateTime FirstDisplayedPeriod { get; set; }

        /// <summary>
        /// Gets or sets the last period displayed on the chart.
        /// </summary>
        public DateTime LastDisplayedPeriod { get; set; }

        /// <summary>
        /// Gets the lower boundary of the price axis (vertical).
        /// </summary>
        public double MinDisplayedPrice
        {
            get { return (double) PriceSeries.PricePeriods.Where(p => p.Head >= FirstDisplayedPeriod && p.Tail <= LastDisplayedPeriod).Min(p => p.Low.Value); }
        }

        /// <summary>
        /// Gets the upper boundary of the price axis (vertical).
        /// </summary>
        public double MaxDisplayedPrice
        {
            get { return (double) PriceSeries.PricePeriods.Where(p => p.Head >= FirstDisplayedPeriod && p.Tail <= LastDisplayedPeriod).Max(p => p.High.Value); }
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

            IOrderedEnumerable<PricePeriod> orderedPeriods =
                PriceSeries.PricePeriods.Where(period => period.Head >= FirstDisplayedPeriod && period.Tail <= LastDisplayedPeriod).OrderByDescending(period => period.Head);
            for (int i = 0; i < orderedPeriods.Count(); i++)
            {
                double open = Convert.ToDouble(orderedPeriods.ElementAt(i).Open.Value);
                double high = Convert.ToDouble(orderedPeriods.ElementAt(i).High.Value);
                double low = Convert.ToDouble(orderedPeriods.ElementAt(i).Low.Value);
                double close = Convert.ToDouble(orderedPeriods.ElementAt(i).Close);
                double lastClose;
                try
                {
                    PricePeriod previous = orderedPeriods.ElementAt(i + 1);
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
            var points = GetPeriodPoints(XNormalize(candlestick), YNormalize(open), YNormalize(high), YNormalize(low), YNormalize(close));

            var candle = new Polyline();
            foreach (var point in points)
            {
                candle.Points.Add(point);
            }

            candle.Stroke = close >= lastClose
                                ? Brushes.Black
                                : Brushes.Red;
            candle.Fill = close >= open
                              ? Brushes.White
                              : candle.Stroke;
            candle.StrokeThickness = StrokeThickness;
            chartCanvas.Children.Add(candle);
        }

        /// <summary>
        /// Gets the points to chart for a period.
        /// </summary>
        /// <param name="center">The center of the period.</param>
        /// <param name="open">The opening price of the period.</param>
        /// <param name="high">The high price of the period.</param>
        /// <param name="low">The low price of the period.</param>
        /// <param name="close">The closing price of the period.</param>
        /// <returns>A <see cref="PointCollection"/> containing the points to be charted for the period.</returns>
        public abstract PointCollection GetPeriodPoints(double center, double open, double high, double low, double close);

        private double XNormalize(double candlePosition)
        {
            double bufferRight = (BufferRight*CandleWidth) + HalfCandleWidth;
            double spacing = CandleSpacing*CandleWidth;
            double offset = candlePosition*(CandleWidth + spacing) + HalfCandleWidth;
            double result = chartCanvas.Width - offset - bufferRight;
            return result;
        }

        private double YNormalize(double price)
        {
            double minimum = MinDisplayedPrice - BufferBottom;
            double maximum = MaxDisplayedPrice + BufferTop;
            double position = (price - minimum)/(maximum - minimum);
            double flippedPosition = chartCanvas.Height - (position*chartCanvas.Height);
            return flippedPosition;
        }
    }
}
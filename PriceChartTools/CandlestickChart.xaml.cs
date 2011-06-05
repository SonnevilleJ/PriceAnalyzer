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
            BufferZone = 1;

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
        private double BufferZone { get; set; }

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

            double open;
            double close;
            double high;
            double low;
            double lastClose = 0;

            var orderedPeriods = PriceSeries.PricePeriods.OrderBy(period =>period.Head);
            for (int i = 0; i < orderedPeriods.Count(); i++)
            {
                open = Convert.ToDouble(orderedPeriods.ElementAt(i).Open.Value);
                high = Convert.ToDouble(orderedPeriods.ElementAt(i).High.Value);
                low = Convert.ToDouble(orderedPeriods.ElementAt(i).Low.Value);
                close = Convert.ToDouble(orderedPeriods.ElementAt(i).Close);

                DrawCandlestick(i, low, open, close, high, lastClose);
                lastClose = close;
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
            var buffer = (BufferZone * CandleWidth) + HalfCandleWidth;
            var spacing = CandleSpacing * CandleWidth;
            var offset = candlePosition*(CandleWidth + spacing) + HalfCandleWidth;
            var result = chartCanvas.Width - offset - buffer;
            return result;
        }

        private double YNormalize(double y)
        {
            var yNormalize = chartCanvas.Height - y*chartCanvas.Height/YMax;
            return yNormalize;
        }
    }
}

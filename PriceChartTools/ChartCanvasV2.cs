using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Sonneville.PriceTools;

namespace Sonneville.PriceChartTools
{
    public class ChartCanvasV2 : Canvas, IChart
    {
        #region Private Members

        private IPriceSeries _priceSeries;

        #endregion

        #region Constructors

        public ChartCanvasV2()
            : this(false)
        {
        }

        public ChartCanvasV2(bool connectPeriods)
        {
            GainStroke = Brushes.Black;
            LossStroke = Brushes.Red;
            GainFill = Brushes.White;
            StrokeThickness = 1;
            MajorGridlineBrush = Brushes.DarkGray;
            MinorGridlineBrush = Brushes.LightGray;
            MajorHorizontalGridlineDistance = 10;
            MinorHorizontalGridlineDistance = 1;
            PeriodWidth = 6;
            PeriodSpacing = 1;
            BufferRight = 1;
            BufferTop = 3;
            BufferBottom = 3;
            ConnectPeriods = connectPeriods;

            //LastDisplayedPeriod = DateTime.Now;
            //FirstDisplayedPeriod = LastDisplayedPeriod.Subtract(new TimeSpan(30, 0, 0, 0));
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> used to outline a gain day. Default: <see cref="Brushes.Black"/>.
        /// </summary>
        public Brush GainStroke { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> used to outline a loss day. Default: <see cref="Brushes.Red"/>.
        /// </summary>
        /// <remarks>This brush will also be used to fill a loss day.</remarks>
        public Brush LossStroke { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> used to fill a gain day. Default: <see cref="Brushes.White"/>.
        /// </summary>
        /// <remarks>Loss days will be filled with the <see cref="IChart.LossStroke"/> brush.</remarks>
        public Brush GainFill { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> used for major gridlines.
        /// </summary>
        public Brush MajorGridlineBrush { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> used for minor gridlines.
        /// </summary>
        public Brush MinorGridlineBrush { get; set; }

        /// <summary>
        /// Gets or sets the distance between major horizontal gridlines.
        /// </summary>
        public int MajorHorizontalGridlineDistance { get; private set; }

        /// <summary>
        /// Gets or sets the distance between minor horizontal gridlines.
        /// </summary>
        public int MinorHorizontalGridlineDistance { get; private set; }

        /// <summary>
        /// Gets or sets the thickness of the stroke used to draw the chart.
        /// </summary>
        public int StrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the periods should be connected.
        /// </summary>
        public bool ConnectPeriods { get; set; }

        /// <summary>
        /// Gets or sets the first period displayed on the chart.
        /// </summary>
        public DateTime FirstDisplayedPeriod
        {
            get { return PriceSeries.Head; }
        }

        /// <summary>
        /// Gets or sets the last period displayed on the chart.
        /// </summary>
        public DateTime LastDisplayedPeriod
        {
            get { return PriceSeries.Tail; }
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
        /// Gets or sets the horizontal space taken up by half a period.
        /// </summary>
        public int HalfPeriodWidth { get; set; }

        /// <summary>
        /// Gets or sets the horizontal space taken up by a full period.
        /// </summary>
        public int PeriodWidth
        {
            get { return HalfPeriodWidth * 2; }
            set { HalfPeriodWidth = value / 2; }
        }

        /// <summary>
        /// Gets or sets the space in period widths that should be placed between periods.
        /// </summary>
        public double PeriodSpacing { get; set; }

        /// <summary>
        /// Gets or sets the space in period widths between the last period and the right edge of the chart.
        /// </summary>
        public double BufferRight { get; set; }

        /// <summary>
        /// Gets or sets the space in period widths between the first period and the left edge of the chart.
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
        /// Gets the lower boundary of the price axis (vertical).
        /// </summary>
        public double MinDisplayedPrice
        {
            get { return (double)PriceSeries.PricePeriods.Where(p => p.Head >= FirstDisplayedPeriod && p.Tail <= LastDisplayedPeriod).Min(p => p.Low); }
        }

        /// <summary>
        /// Gets the upper boundary of the price axis (vertical).
        /// </summary>
        public double MaxDisplayedPrice
        {
            get { return (double)PriceSeries.PricePeriods.Where(p => p.Head >= FirstDisplayedPeriod && p.Tail <= LastDisplayedPeriod).Max(p => p.High); }
        }

        #endregion

        #region Draw Methods

        private void DrawChart()
        {
            if (PriceSeries == null) return;
            Children.Clear();

            var orderedPeriods = PriceSeries.PricePeriods.Where(period => period.Head >= FirstDisplayedPeriod && period.Tail <= LastDisplayedPeriod).OrderByDescending(period => period.Head);
            for (var i = 0; i < orderedPeriods.Count(); i++)
            {
                var pricePeriod = orderedPeriods.ElementAt(i);
                var decOpen = pricePeriod.Open;
                var decHigh = pricePeriod.High;
                var decLow = pricePeriod.Low;

                var open = Convert.ToDouble(decOpen);
                var high = Convert.ToDouble(decHigh);
                var low = Convert.ToDouble(decLow);
                var close = Convert.ToDouble(pricePeriod.Close);
                double previousClose;
                try
                {
                    var previous = orderedPeriods.ElementAt(i + 1);
                    previousClose = Convert.ToDouble(previous.Close);
                }
                catch (ArgumentOutOfRangeException)
                {
                    previousClose = 0;
                }

                var polyline = FormPolyline(i, open, high, low, close, previousClose);

                if (ConnectPeriods && previousClose != 0)
                {
                    var previousX = XNormalize(i + 1);
                    var previousY = YNormalize(previousClose);
                    polyline.Points.Insert(0, new Point(previousX, previousY));
                }
                Children.Add(polyline);
            }
        }

        private Polyline FormPolyline(double period, double open, double high, double low, double close, double previousClose)
        {
            var center = XNormalize(period);
            var nClose = YNormalize(close);

            var stroke = close >= previousClose ? GainStroke : LossStroke;
            var fill = close >= open ? GainFill : stroke;
            var left = center - HalfPeriodWidth;
            var right = center + HalfPeriodWidth;

            return new Polyline
            {
                Points = GetPolylinePoints(left, center, right, YNormalize(open), YNormalize(high), YNormalize(low), nClose),
                Stroke = stroke,
                Fill = fill,
                StrokeThickness = StrokeThickness
            };
        }

        private double XNormalize(double period)
        {
            var bufferRight = (BufferRight * PeriodWidth) + HalfPeriodWidth;
            var spacing = PeriodSpacing * PeriodWidth;
            var offset = period * (PeriodWidth + spacing) + HalfPeriodWidth;
            var result = ActualWidth - offset - bufferRight;
            return result;
        }

        private double YNormalize(double price)
        {
            var minimum = MinDisplayedPrice - BufferBottom;
            var maximum = MaxDisplayedPrice + BufferTop;
            var range = maximum - minimum;
            var priceOverMinimum = price - minimum;
            var position = priceOverMinimum / range;
            var flippedPosition = ActualHeight - (position * ActualHeight);
            return flippedPosition;
        }

        #endregion

        #region Abstract/Virtual Methods

        /// <summary>
        /// When overridden in a class, constructs a <see cref="PointCollection"/> from X,Y coordinate points which defines the visual shape of an <see cref="IPricePeriod"/>.
        /// The base implementation draws only the closing price, resulting in a line chart.
        /// </summary>
        /// <param name="left">The X coordinate of the left edge of the period.</param>
        /// <param name="center">The X coordinate of the center of the period.</param>
        /// <param name="right">The X coordinate of the right edge of the period.</param>
        /// <param name="open">The Y coordinate of the period's opening price.</param>
        /// <param name="high">The Y coordinate of the period's highest price.</param>
        /// <param name="low">The Y coordinate of the period's lowest price.</param>
        /// <param name="close">The Y coordinate of the period's closing price.</param>
        /// <returns>A <see cref="PointCollection"/> which defines the visual shape of an <see cref="IPricePeriod"/>.</returns>
        protected virtual PointCollection GetPolylinePoints(double left, double center, double right, double open, double high, double low, double close)
        {
            return new PointCollection { new Point(center, close) };
        }

        #endregion
    }
}

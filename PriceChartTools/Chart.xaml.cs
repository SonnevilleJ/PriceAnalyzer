using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Sonneville.PriceTools;

namespace Sonneville.PriceChartTools
{
    /// <summary>
    /// A graphical chart used to display price history.
    /// </summary>
    public abstract partial class Chart : IChart
    {
        #region Private Members

        private IPriceSeries _priceSeries;

        #endregion

        #region Constructors

        protected Chart()
            : this(false)
        {
        }

        protected Chart(bool connectPeriods)
        {
            InitializeComponent();

            SetChartDefaults(connectPeriods);
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

                EnsurePricesVisible();
                DrawChart();
            }
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
        /// Gets or sets the horizontal space taken up by half a period.
        /// </summary>
        public int HalfPeriodWidth { get; set; }

        /// <summary>
        /// Gets or sets the horizontal space taken up by a full period.
        /// </summary>
        public int PeriodWidth
        {
            get { return HalfPeriodWidth*2; }
            set { HalfPeriodWidth = value/2; }
        }

        /// <summary>
        /// Gets or sets a value indicating if the periods should be connected.
        /// </summary>
        public bool ConnectPeriods { get; set; }

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
            get { return (double) PriceSeries.PricePeriods.Where(p => p.Head >= FirstDisplayedPeriod && p.Tail <= LastDisplayedPeriod).Min(p => p.Low ?? 0); }
        }

        /// <summary>
        /// Gets the upper boundary of the price axis (vertical).
        /// </summary>
        public double MaxDisplayedPrice
        {
            get { return (double) PriceSeries.PricePeriods.Where(p => p.Head >= FirstDisplayedPeriod && p.Tail <= LastDisplayedPeriod).Max(p => p.High ?? 100); }
        }

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

        #endregion

        #region Draw Methods

        private void DrawChart()
        {
            if (PriceSeries == null) return;

            chartCanvas.Children.Clear();
            DrawGridlines();

            IOrderedEnumerable<PricePeriod> orderedPeriods =
                PriceSeries.PricePeriods.Where(period => period.Head >= FirstDisplayedPeriod && period.Tail <= LastDisplayedPeriod).OrderByDescending(period => period.Head);
            for (int i = 0; i < orderedPeriods.Count(); i++)
            {
                var pricePeriod = orderedPeriods.ElementAt(i);
                var decOpen = pricePeriod.Open;
                var decHigh = pricePeriod.High;
                var decLow = pricePeriod.Low;

                var open = decOpen.HasValue ? Convert.ToDouble(decOpen.Value) : (double?) null;
                var high = decHigh.HasValue ? Convert.ToDouble(decHigh.Value) : (double?) null;
                var low = decLow.HasValue ? Convert.ToDouble(decLow.Value) : (double?) null;
                var close = Convert.ToDouble(pricePeriod.Close);
                double previousClose;
                try
                {
                    PricePeriod previous = orderedPeriods.ElementAt(i + 1);
                    previousClose = Convert.ToDouble(previous.Close);
                }
                catch (ArgumentOutOfRangeException)
                {
                    previousClose = 0;
                }

                Polyline polyline = FormPolyline(i, open, high, low, close, previousClose);

                if (ConnectPeriods && previousClose != 0)
                {
                    var previousX = XNormalize(i + 1);
                    var previousY = YNormalize(previousClose);
                    if (previousX.HasValue && previousY.HasValue)
                    {
                        polyline.Points.Insert(0, new Point(previousX.Value, previousY.Value));
                    }
                }
                chartCanvas.Children.Add(polyline);
            }
        }

        /// <summary>
        /// Draws all gridlines based on the visible prices.
        /// </summary>
        private void DrawGridlines()
        {
            var lowerHorizontal = NormalizeGridline(PriceSeries.Low * 0.90m);
            var upperHorizontal = NormalizeGridline(PriceSeries.High * 1.10m);

            for (var i = lowerHorizontal; i <= upperHorizontal; i += MinorHorizontalGridlineDistance)
            {
                DrawHorizontalLine(i, MinorGridlineBrush);
            }

            for (var i = lowerHorizontal; i <= upperHorizontal; i += MajorHorizontalGridlineDistance)
            {
                DrawHorizontalLine(i, MajorGridlineBrush);
            }
        }

        private int NormalizeGridline(decimal? value)
        {
            return (Convert.ToInt32(value) / MinorHorizontalGridlineDistance) * MinorHorizontalGridlineDistance;
        }

        private Polyline FormPolyline(double period, double? open, double? high, double? low, double close, double previousClose)
        {
            var center = XNormalize(period);
            if (!center.HasValue) { throw new ArgumentNullException("period"); }
            var nClose = YNormalize(close);
            if(!nClose.HasValue) { throw new ArgumentNullException("close"); }

            var stroke = close >= previousClose ? GainStroke : LossStroke;
            var fill = close >= open ? GainFill : stroke;
            double left = center.Value - HalfPeriodWidth;
            double right = center.Value + HalfPeriodWidth;

            return new Polyline
                       {
                           Points = GetPolylinePoints(left, center.Value, right, YNormalize(open), YNormalize(high), YNormalize(low), nClose.Value),
                           Stroke = stroke,
                           Fill = fill,
                           StrokeThickness = StrokeThickness
                       };
        }

        /// <summary>
        /// Gets the polyline points to chart for a period.
        /// </summary>
        /// <param name="left">The left edge of the period.</param>
        /// <param name="center">The center of the period.</param>
        /// <param name="right">The right edge of the period.</param>
        /// <param name="open">The opening price of the period.</param>
        /// <param name="high">The high price of the period.</param>
        /// <param name="low">The low price of the period.</param>
        /// <param name="close">The closing price of the period.</param>
        /// <returns>A <see cref="PointCollection"/> containing the points to be charted for the period.</returns>
        public abstract PointCollection GetPolylinePoints(double left, double center, double right, double? open, double? high, double? low, double close);

        private void DrawHorizontalLine(double y, Brush lineColor)
        {
            var yNormalized = YNormalize(y);
            if (yNormalized.HasValue && !Double.IsNaN(yNormalized.Value))
            {
                var line = new Line
                               {
                                   X1 = 0,
                                   X2 = chartCanvas.Width,
                                   Y1 = yNormalized.Value,
                                   Y2 = yNormalized.Value,
                                   Stroke = lineColor
                               };

                chartCanvas.Children.Add(line);
            }
        }

        #endregion

        #region Private Methods

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

        private void EnsurePricesVisible()
        {
            //if (!(PriceSeries.HasValue(LastDisplayedPeriod) || PriceSeries.HasValue(FirstDisplayedPeriod)))
            //{
            //    FirstDisplayedPeriod = PriceSeries.Head;
            //    LastDisplayedPeriod = PriceSeries.Tail;
            //}
        }

        private double? XNormalize(double period)
        {
            double bufferRight = (BufferRight*PeriodWidth) + HalfPeriodWidth;
            double spacing = PeriodSpacing*PeriodWidth;
            double offset = period*(PeriodWidth + spacing) + HalfPeriodWidth;
            double result = chartCanvas.Width - offset - bufferRight;
            return result;
        }

        private double? YNormalize(double? price)
        {
            if (!price.HasValue) return null;

            double minimum = MinDisplayedPrice - BufferBottom;
            double maximum = MaxDisplayedPrice + BufferTop;
            var range = maximum - minimum;
            var priceOverMinimum = price.Value - minimum;
            double position = priceOverMinimum/range;
            double flippedPosition = chartCanvas.Height - (position*chartCanvas.Height);
            return flippedPosition;
        }

        private void SetChartDefaults(bool connectPeriods)
        {
            GainStroke = Brushes.Black;
            LossStroke = Brushes.Red;
            GainFill = Brushes.White;
            MajorGridlineBrush = Brushes.DarkGray;
            MinorGridlineBrush = Brushes.LightGray;
            MajorHorizontalGridlineDistance = 10;
            MinorHorizontalGridlineDistance = 1;
            StrokeThickness = 1;
            PeriodWidth = 6;
            PeriodSpacing = 1;
            BufferRight = 1;
            BufferTop = 3;
            BufferBottom = 3;
            ConnectPeriods = connectPeriods;

            LastDisplayedPeriod = DateTime.Now;
            FirstDisplayedPeriod = LastDisplayedPeriod.Subtract(new TimeSpan(30, 0, 0, 0));
        }

        #endregion
    }
}
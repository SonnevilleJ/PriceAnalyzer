using System;
using System.Linq;
using System.Windows;
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

            PriceSeries priceSeries = PriceSeriesFactory.CreatePriceSeries("DE");
            priceSeries.DownloadPriceData(new DateTime(2011, 4, 1));
            FirstDisplayedPeriod = priceSeries.Head;
            LastDisplayedPeriod = priceSeries.Tail;

            PriceSeries = priceSeries;
        }

        private void SetChartDefaults(bool connectPeriods)
        {
            GainStroke = Brushes.Black;
            LossStroke = Brushes.Red;
            GainFill = Brushes.White;
            StrokeThickness = 1;
            PeriodWidth = 6;
            PeriodSpacing = 1;
            BufferRight = 1;
            BufferTop = 5;
            BufferBottom = 10;
            ConnectPeriods = connectPeriods;
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
            get { return (double) PriceSeries.PricePeriods.Where(p => p.Head >= FirstDisplayedPeriod && p.Tail <= LastDisplayedPeriod).Min(p => p.Low.Value); }
        }

        /// <summary>
        /// Gets the upper boundary of the price axis (vertical).
        /// </summary>
        public double MaxDisplayedPrice
        {
            get { return (double) PriceSeries.PricePeriods.Where(p => p.Head >= FirstDisplayedPeriod && p.Tail <= LastDisplayedPeriod).Max(p => p.High.Value); }
        }

        #endregion

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

        #region Draw Methods

        private void DrawChart()
        {
            chartCanvas.Children.Clear();

            IOrderedEnumerable<PricePeriod> orderedPeriods =
                PriceSeries.PricePeriods.Where(period => period.Head >= FirstDisplayedPeriod && period.Tail <= LastDisplayedPeriod).OrderByDescending(period => period.Head);
            Point? lastPoint = null;
            for (int i = 0; i < orderedPeriods.Count(); i++)
            {
                double open = Convert.ToDouble(orderedPeriods.ElementAt(i).Open.Value);
                double high = Convert.ToDouble(orderedPeriods.ElementAt(i).High.Value);
                double low = Convert.ToDouble(orderedPeriods.ElementAt(i).Low.Value);
                double close = Convert.ToDouble(orderedPeriods.ElementAt(i).Close);
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
                    if (previousX.HasValue)
                    {
                        polyline.Points.Insert(0, new Point(previousX.Value, previousY.Value));
                    }
                }
                chartCanvas.Children.Add(polyline);
            }
        }

        private Polyline FormPolyline(double period, double open, double high, double low, double close, double previousClose)
        {
            var center = XNormalize(period);
            if (!center.HasValue) { throw new ArgumentNullException("period"); }
            var nClose = YNormalize(close);
            if(!nClose.HasValue) { throw new ArgumentNullException("close"); }

            var stroke = close >= previousClose ? GainStroke : LossStroke;
            var fill = close >= open ? GainFill : stroke;
            var polyline = new Polyline
                               {
                                   Points = GetPolylinePoints(center.Value, YNormalize(open), YNormalize(high), YNormalize(low), nClose.Value),
                                   Stroke = stroke,
                                   Fill = fill,
                                   StrokeThickness = StrokeThickness
                               };

            return polyline;
        }

        /// <summary>
        /// Gets the polyline points to chart for a period.
        /// </summary>
        /// <param name="center">The center of the period.</param>
        /// <param name="open">The opening price of the period.</param>
        /// <param name="high">The high price of the period.</param>
        /// <param name="low">The low price of the period.</param>
        /// <param name="close">The closing price of the period.</param>
        /// <returns>A <see cref="PointCollection"/> containing the points to be charted for the period.</returns>
        public abstract PointCollection GetPolylinePoints(double center, double? open, double? high, double? low, double close);

        #endregion

        #region Normalization methods

        private double? XNormalize(double period)
        {
            double bufferRight = (BufferRight*PeriodWidth) + HalfPeriodWidth;
            double spacing = PeriodSpacing*PeriodWidth;
            double offset = period*(PeriodWidth + spacing) + HalfPeriodWidth;
            double result = chartCanvas.Width - offset - bufferRight;
            return result;
        }

        private double? YNormalize(double price)
        {
            double minimum = MinDisplayedPrice - BufferBottom;
            double maximum = MaxDisplayedPrice + BufferTop;
            double position = (price - minimum)/(maximum - minimum);
            double flippedPosition = chartCanvas.Height - (position*chartCanvas.Height);
            return flippedPosition;
        }

        #endregion
    }
}
//     
//     Copyright 2011 John Sonneville
//     
using System.Windows;
using System.Windows.Media;
using LineCharts;

namespace Sonneville.PriceChartTools
{
    /// <summary>
    ///   Interaction logic for LineChartControl.xaml
    /// </summary>
    public partial class LineChartControl : Chart
    {
        public static DependencyProperty XminProperty =
            DependencyProperty.Register("Xmin", typeof (double), typeof (LineChartControl),
                                        new FrameworkPropertyMetadata(0.0,
                                                                      new PropertyChangedCallback(OnPropertyChanged)));

        public static DependencyProperty XmaxProperty =
            DependencyProperty.Register("Xmax", typeof (double), typeof (LineChartControl),
                                        new FrameworkPropertyMetadata(10.0,
                                                                      new PropertyChangedCallback(OnPropertyChanged)));

        public static DependencyProperty YminProperty =
            DependencyProperty.Register("Ymin", typeof (double), typeof (LineChartControl),
                                        new FrameworkPropertyMetadata(0.0,
                                                                      new PropertyChangedCallback(OnPropertyChanged)));

        public static DependencyProperty YmaxProperty =
            DependencyProperty.Register("Ymax", typeof (double), typeof (LineChartControl),
                                        new FrameworkPropertyMetadata(10.0,
                                                                      new PropertyChangedCallback(OnPropertyChanged)));

        public static DependencyProperty XTickProperty =
            DependencyProperty.Register("XTick", typeof (double), typeof (LineChartControl),
                                        new FrameworkPropertyMetadata(2.0,
                                                                      new PropertyChangedCallback(OnPropertyChanged)));

        public static DependencyProperty YTickProperty =
            DependencyProperty.Register("YTick", typeof (double), typeof (LineChartControl),
                                        new FrameworkPropertyMetadata(2.0,
                                                                      new PropertyChangedCallback(OnPropertyChanged)));

        public static DependencyProperty XLabelProperty =
            DependencyProperty.Register("XLabel", typeof (string), typeof (LineChartControl),
                                        new FrameworkPropertyMetadata("X Axis",
                                                                      new PropertyChangedCallback(OnPropertyChanged)));

        public static DependencyProperty YLabelProperty =
            DependencyProperty.Register("YLabel", typeof (string), typeof (LineChartControl),
                                        new FrameworkPropertyMetadata("Y Axis",
                                                                      new PropertyChangedCallback(OnPropertyChanged)));

        public static DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof (string), typeof (LineChartControl),
                                        new FrameworkPropertyMetadata("Title",
                                                                      new PropertyChangedCallback(OnPropertyChanged)));

        public static DependencyProperty IsXGridProperty =
            DependencyProperty.Register("IsXGrid", typeof (bool), typeof (LineChartControl),
                                        new FrameworkPropertyMetadata(true,
                                                                      new PropertyChangedCallback(OnPropertyChanged)));

        public static DependencyProperty IsYGridProperty =
            DependencyProperty.Register("IsYGrid", typeof (bool), typeof (LineChartControl),
                                        new FrameworkPropertyMetadata(true,
                                                                      new PropertyChangedCallback(OnPropertyChanged)));

        public static DependencyProperty GridlineColorProperty =
            DependencyProperty.Register("GridlineColor", typeof (Brush), typeof (LineChartControl),
                                        new FrameworkPropertyMetadata(Brushes.Gray,
                                                                      new PropertyChangedCallback(OnPropertyChanged)));

        public static DependencyProperty GridlinePatternProperty =
            DependencyProperty.Register("GridlinePattern", typeof (ChartStyleGridlines.GridlinePatternEnum),
                                        typeof (LineChartControl),
                                        new FrameworkPropertyMetadata(ChartStyleGridlines.GridlinePatternEnum.Solid,
                                                                      new PropertyChangedCallback(OnPropertyChanged)));

        public static DependencyProperty IsLegendProperty =
            DependencyProperty.Register("IsLegend", typeof (bool), typeof (LineChartControl),
                                        new FrameworkPropertyMetadata(false,
                                                                      new PropertyChangedCallback(OnPropertyChanged)));

        public static DependencyProperty LegendPositionProperty =
            DependencyProperty.Register("Position", typeof (CardinalDirection), typeof (LineChartControl),
                                        new FrameworkPropertyMetadata(CardinalDirection.NorthEast,
                                                                      new PropertyChangedCallback(OnPropertyChanged)));

        private ChartStyleGridlines cs;
        private Legend lg;

        public LineChartControl()
        {
            InitializeComponent();

            cs = new ChartStyleGridlines();
            DataCollection = new DataCollection();
            lg = new Legend();
            cs.TextCanvas = textCanvas;
            cs.ChartCanvas = chartCanvas;
            lg.Canvas = legendCanvas;
        }

        public ChartStyleGridlines ChartStyle
        {
            get { return cs; }
            set { cs = value; }
        }

        public DataCollection DataCollection { get; set; }

        public DataSeries DataSeries { get; set; }

        public Legend Legend
        {
            get { return lg; }
            set { lg = value; }
        }

        public double Xmin
        {
            get { return (double) GetValue(XminProperty); }
            set
            {
                SetValue(XminProperty, value);
                cs.Xmin = value;
            }
        }


        public double Xmax
        {
            get { return (double) GetValue(XmaxProperty); }
            set
            {
                SetValue(XmaxProperty, value);
                cs.Xmax = value;
            }
        }


        public double Ymin
        {
            get { return (double) GetValue(YminProperty); }
            set
            {
                SetValue(YminProperty, value);
                cs.Ymin = value;
            }
        }


        public double Ymax
        {
            get { return (double) GetValue(YmaxProperty); }
            set
            {
                SetValue(YmaxProperty, value);
                cs.Ymax = value;
            }
        }


        public double XTick
        {
            get { return (double) GetValue(XTickProperty); }
            set
            {
                SetValue(XTickProperty, value);
                cs.XTick = value;
            }
        }


        public double YTick
        {
            get { return (double) GetValue(YTickProperty); }
            set
            {
                SetValue(YTickProperty, value);
                cs.YTick = value;
            }
        }


        public string XLabel
        {
            get { return (string) GetValue(XLabelProperty); }
            set
            {
                SetValue(XLabelProperty, value);
                cs.XLabel = value;
            }
        }


        public string YLabel
        {
            get { return (string) GetValue(YLabelProperty); }
            set
            {
                SetValue(YLabelProperty, value);
                cs.YLabel = value;
            }
        }


        public string Title
        {
            get { return (string) GetValue(TitleProperty); }
            set
            {
                SetValue(TitleProperty, value);
                cs.Title = value;
            }
        }


        public bool IsXGrid
        {
            get { return (bool) GetValue(IsXGridProperty); }
            set
            {
                SetValue(IsXGridProperty, value);
                cs.IsXGrid = value;
            }
        }


        public bool IsYGrid
        {
            get { return (bool) GetValue(IsYGridProperty); }
            set
            {
                SetValue(IsYGridProperty, value);
                cs.IsYGrid = value;
            }
        }


        public Brush GridlineColor
        {
            get { return (Brush) GetValue(GridlineColorProperty); }
            set
            {
                SetValue(GridlineColorProperty, value);
                cs.GridlineColor = value;
            }
        }


        public ChartStyleGridlines.GridlinePatternEnum GridlinePattern
        {
            get { return (ChartStyleGridlines.GridlinePatternEnum) GetValue(GridlinePatternProperty); }
            set
            {
                SetValue(GridlinePatternProperty, value);
                cs.GridlinePattern = value;
            }
        }


        public bool IsLegend
        {
            get { return (bool) GetValue(IsLegendProperty); }
            set
            {
                SetValue(IsLegendProperty, value);
                lg.IsLegend = value;
            }
        }


        protected CardinalDirection LegendPosition
        {
            get { return (CardinalDirection) GetValue(LegendPositionProperty); }
            set
            {
                SetValue(LegendPositionProperty, value);
                lg.Position = value;
            }
        }

        private void chartGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            textCanvas.Width = chartGrid.ActualWidth;
            textCanvas.Height = chartGrid.Height;
            legendCanvas.Children.Clear();
            chartCanvas.Children.RemoveRange(1, chartCanvas.Children.Count - 1);
            textCanvas.Children.RemoveRange(1, textCanvas.Children.Count - 1);

            AddChart();
        }

        private void AddChart()
        {
            cs.AddChartStyle(tbTitle, tbXLabel, tbYLabel);

            if (DataCollection.DataList.Count != 0)
            {
                DataCollection.AddLines(cs);
                Legend.AddLegend(chartCanvas, DataCollection);
            }
        }


        private static void OnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            LineChartControl lcc = sender as LineChartControl;
            if (e.Property == XminProperty)
                lcc.Xmin = (double) e.NewValue;
            else if (e.Property == XmaxProperty)
                lcc.Xmax = (double) e.NewValue;
            else if (e.Property == YminProperty)
                lcc.Ymin = (double) e.NewValue;
            else if (e.Property == YmaxProperty)
                lcc.Ymax = (double) e.NewValue;
            else if (e.Property == XTickProperty)
                lcc.XTick = (double) e.NewValue;
            else if (e.Property == YTickProperty)
                lcc.YTick = (double) e.NewValue;
            else if (e.Property == GridlinePatternProperty)
                lcc.GridlinePattern = (ChartStyleGridlines.GridlinePatternEnum) e.NewValue;
            else if (e.Property == GridlineColorProperty)
                lcc.GridlineColor = (Brush) e.NewValue;
            else if (e.Property == TitleProperty)
                lcc.Title = (string) e.NewValue;
            else if (e.Property == XLabelProperty)
                lcc.XLabel = (string) e.NewValue;
            else if (e.Property == YLabelProperty)
                lcc.YLabel = (string) e.NewValue;
            else if (e.Property == IsXGridProperty)
                lcc.IsXGrid = (bool) e.NewValue;
            else if (e.Property == IsYGridProperty)
                lcc.IsYGrid = (bool) e.NewValue;
            else if (e.Property == IsLegendProperty)
                lcc.IsLegend = (bool) e.NewValue;
            else if (e.Property == LegendPositionProperty)
                lcc.LegendPosition = (CardinalDirection) e.NewValue;
        }
    }
}
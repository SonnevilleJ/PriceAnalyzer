using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sonneville.PriceTools;

namespace Sonneville.PriceChartTools
{
    /// <summary>
    /// Interaction logic for LineChart.xaml
    /// </summary>
    public partial class LineChart : UserControl
    {
        private ITimeSeries _timeSeries;

        public LineChart()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Routed event that is raised when the TimeSeries property has changed.
        /// </summary>
        public RoutedEvent TimeSeriesUpdated;
        
        public ITimeSeries TimeSeries
        {
            get
            {
                return _timeSeries;
            }
            set
            {
                _timeSeries = value;
                RaiseEvent(new RoutedEventArgs {RoutedEvent = TimeSeriesUpdated, Source = TimeSeries});
            }
        }

        public void Chart(DateTime head, DateTime tail)
        {
            DrawAxis(head, tail);
        }

        private void DrawAxis(DateTime head, DateTime tail)
        {
            DrawingImage di = new DrawingImage();
            PathGeometry geometry;

        }
    }
}

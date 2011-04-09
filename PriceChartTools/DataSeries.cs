using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Sonneville.PriceChartTools;

namespace LineCharts
{
    public class DataSeries
    {
        private Polyline lineSeries = new Polyline();
        private Brush lineColor;
        private double lineThickness = 1;
        private LinePattern linePattern;
        private string _ticker = "Default Name";
        private Symbols symbols;

        public DataSeries()
        {
            LineColor = Brushes.Black;
            symbols = new Symbols();
        }

        public Symbols Symbols
        {
            get { return symbols; }
            set { symbols = value; }
        }

        public Brush LineColor
        {
            get { return lineColor; }
            set { lineColor = value; }
        }

        public Polyline LineSeries
        {
            get { return lineSeries; }
            set { lineSeries = value; }
        }

        public double LineThickness
        {
            get { return lineThickness; }
            set { lineThickness = value; }
        }

        public LinePattern LinePattern
        {
            get { return linePattern; }
            set { linePattern = value; }
        }

        public string Ticker
        {
            get { return _ticker; }
            set { _ticker = value; }
        }

        public void AddLinePattern()
        {
            LineSeries.Stroke = LineColor;
            LineSeries.StrokeThickness = LineThickness;

            switch (LinePattern)
            {
                case LinePattern.Dash:
                    LineSeries.StrokeDashArray = new DoubleCollection(new double[2] { 4, 3 });
                    break;
                case LinePattern.Dot:
                    LineSeries.StrokeDashArray = new DoubleCollection(new double[2] { 1, 2 });
                    break;
                case LinePattern.DashDot:
                    LineSeries.StrokeDashArray = new DoubleCollection(new double[4] { 4, 2, 1, 2 });
                    break;
                case LinePattern.None:
                    LineSeries.Stroke = Brushes.Transparent;
                    break;
            }
        }
    }
}

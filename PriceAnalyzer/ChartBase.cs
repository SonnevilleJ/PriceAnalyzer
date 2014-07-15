using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public abstract class ChartBase : Grid, IChart
    {
        protected IList<IPricePeriod> _pricePeriods;
        protected readonly Canvas _canvas = new Canvas();
        private readonly Canvas _verticalCanvas = new Canvas();
        private readonly Canvas _horizontalCanvas = new Canvas();

        public ChartBase()
        {
            var column0 = new ColumnDefinition();
            column0.Width = new GridLength(45);
            ColumnDefinitions.Add(column0);
            ColumnDefinitions.Add(new ColumnDefinition());

            var row0 = new RowDefinition();
            row0.Height = new GridLength(8);
            RowDefinitions.Add(row0);
            RowDefinitions.Add(new RowDefinition());
            var row2 = new RowDefinition();
            row2.Height = new GridLength(20);
            RowDefinitions.Add(row2);

            Grid.SetColumn(_verticalCanvas, 0);
            Grid.SetRow(_verticalCanvas, 1);
            Grid.SetColumn(_canvas, 1);
            Grid.SetRow(_canvas, 1);
            Grid.SetColumn(_horizontalCanvas, 1);
            Grid.SetRow(_horizontalCanvas, 2);

            Children.Add(_canvas);
            Children.Add(_verticalCanvas);
            Children.Add(_horizontalCanvas);
            SizeChanged += delegate { if (_pricePeriods != null) DrawChart(); };
        }

        public void DrawPricePeriods(IList<IPricePeriod> pricePeriods)
        {
            _pricePeriods = pricePeriods;
            DrawChart();
        }

        private void DrawChart()
        {
            var highestHigh = (double) _pricePeriods.Max(pricePeriod => pricePeriod.High);
            var lowestLow = (double) _pricePeriods.Min(pricePeriod => pricePeriod.Low);

            var dollarRange = (highestHigh - lowestLow);
            var pixelsPerDollar = (_canvas.ActualHeight/dollarRange);
            DrawVerticalAxis(highestHigh, lowestLow, pixelsPerDollar);

            var firstDay = _pricePeriods.Min(pricePeriod => pricePeriod.Head);
            var lastDay = _pricePeriods.Max(pricePeriod => pricePeriod.Tail).AddDays(1);
            var pixelsPerDay = _canvas.ActualWidth/((lastDay - firstDay).Days + 1);
            DrawHorizontalAxis(firstDay, lastDay, pixelsPerDay);
            
            DrawCanvas(highestHigh, lowestLow, pixelsPerDollar, pixelsPerDay);
        }

        private void DrawHorizontalAxis(DateTime firstDay, DateTime lastDay, double pixelsPerValue)
        {
            _horizontalCanvas.Children.Clear();

            const int minimumPixelsPerTick = 60;
            const double maximumTicks = 10;
            var pixelsPerTick = Math.Max(_horizontalCanvas.ActualWidth / maximumTicks, minimumPixelsPerTick);
            var valuesBetweenTicks = pixelsPerTick / pixelsPerValue;
            for (var i = firstDay; i < lastDay; i = i.AddDays(valuesBetweenTicks))
            {
                var location = (i - firstDay).Days*pixelsPerValue + (.5*pixelsPerValue);
                DrawLine(location, 0, 5, 0, _horizontalCanvas);
                DrawValue(i.ToShortDateString(), 7, location - 25, _horizontalCanvas);
            }
            var separator = CreateLine(0, 0, _horizontalCanvas.ActualWidth, 0, Brushes.DarkBlue);
            _horizontalCanvas.Children.Add(separator);
        }

        private void DrawVerticalAxis(double highestHigh, double lowestLow, double pixelsPerValue)
        {
            _verticalCanvas.Children.Clear();

            const int minimumPixelsPerTick = 30;
            const double maximumTicks = 10;
            var pixelsPerTick = Math.Max(_verticalCanvas.ActualHeight/maximumTicks, minimumPixelsPerTick);
            var valuesBetweenTicks = pixelsPerTick / pixelsPerValue;
            for (var i = 0.0; i < Math.Round(highestHigh - lowestLow, 0); i += valuesBetweenTicks)
            {
                var location = i*pixelsPerValue;
                DrawLine(40, location, 0, 5, _verticalCanvas);
                DrawValue((highestHigh - i).ToString("C"), location - (6), 5, _verticalCanvas);
            }
            var separator = CreateLine(_verticalCanvas.ActualWidth, 0, _verticalCanvas.ActualWidth, _verticalCanvas.ActualHeight, Brushes.DarkBlue);
            _verticalCanvas.Children.Add(separator);
        }

        private static void DrawValue(string valueText, double top, double left, Panel panel)
        {
            var textBlock = new TextBlock {Text = valueText, Foreground = Brushes.Black};
            Canvas.SetTop(textBlock, top);
            Canvas.SetLeft(textBlock, left);
            panel.Children.Add(textBlock);
        }

        private static void DrawLine(double xLocation, double yLocation, int tickHeight, int tickWidth, Panel panel)
        {
            var line = CreateLine(xLocation, yLocation, xLocation + tickWidth, yLocation + tickHeight, Brushes.Black);
            panel.Children.Add(line);
        }

        protected static Brush CloseColorBrush(decimal priorPeriodClose, decimal currentPeriodClose)
        {
            if (priorPeriodClose < currentPeriodClose)
            {
                return Brushes.Black;
            }
            else
            {
                return Brushes.Firebrick;
            }
        }

        protected static Line CreateLine(double x1, double y1, double x2, double y2, Brush stroke)
        {
            return new Line {X1 = x1, Y1 = y1, X2 = x2, Y2 = y2, Stroke = stroke};
        }

        protected abstract void DrawCanvas(double maxYdollar, double minYdollar, double pixelsPerDollar, double pixelsPerDay);
    }
}
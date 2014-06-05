using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public class HighLowChart : Grid
    {
        private IList<IPricePeriod> _pricePeriods;
        private readonly Canvas _canvas = new Canvas();
        private readonly Canvas _verticalCanvas = new Canvas();
        private readonly Canvas _horizontalCanvas = new Canvas();

        public HighLowChart()
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
            var lastDay = _pricePeriods.Max(pricePeriod => pricePeriod.Tail);
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
                var location = (i - firstDay).Days*pixelsPerValue;
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

        private void DrawCanvas(double maxYdollar, double minYdollar, double pixelsPerDollar, double pixelsPerDay)
        {
            _canvas.Children.Clear();

            var minX = _pricePeriods.First().Head;

            foreach (var pricePeriod in _pricePeriods)
            {
                var x = ((pricePeriod.Head - minX).Days*pixelsPerDay) + (.5*pixelsPerDay);
                var highLowBar = CreateLine(x, _canvas.ActualHeight - ((double) pricePeriod.Low - minYdollar)*pixelsPerDollar, x, (maxYdollar - (double) pricePeriod.High)*pixelsPerDollar, Brushes.Blue);
                var openY = _canvas.ActualHeight - ((double)pricePeriod.Open - minYdollar)*pixelsPerDollar;
                var openBar = CreateLine(x - (pixelsPerDay*.25), openY, x, openY, Brushes.Blue);
                var closeY = _canvas.ActualHeight - ((double)pricePeriod.Close - minYdollar) * pixelsPerDollar;
                var closeBar = CreateLine(x + (pixelsPerDay*.25), closeY, x, closeY, Brushes.Blue);

                _canvas.Children.Add(highLowBar);
                _canvas.Children.Add(openBar);
                _canvas.Children.Add(closeBar);
            }
        }

        private static Line CreateLine(double x1, double y1, double x2, double y2, Brush stroke)
        {
            return new Line {X1 = x1, Y1 = y1, X2 = x2, Y2 = y2, Stroke = stroke};
        }
    }
}
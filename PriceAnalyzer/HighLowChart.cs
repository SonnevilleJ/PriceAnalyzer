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
            var column1 = new ColumnDefinition();
            column1.Width = new GridLength(45);
            ColumnDefinitions.Add(column1);
            ColumnDefinitions.Add(new ColumnDefinition());
            RowDefinitions.Add(new RowDefinition());
            var row2 = new RowDefinition();
            row2.Height = new GridLength(20);
            RowDefinitions.Add(row2);
            Grid.SetColumn(_verticalCanvas, 0);
            Grid.SetRow(_verticalCanvas, 0);
            Grid.SetColumn(_canvas, 1);
            Grid.SetRow(_canvas, 0);
            Grid.SetColumn(_horizontalCanvas, 1);
            Grid.SetRow(_horizontalCanvas, 1);

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
            var pixelsPerDay = _canvas.ActualWidth / (lastDay - firstDay).Days;
            DrawHorizontalAxis(firstDay, lastDay, pixelsPerDay);
            DrawCanvas(highestHigh, lowestLow, pixelsPerDollar, pixelsPerDay);
        }

        private void DrawHorizontalAxis(DateTime firstDay, DateTime lastDay, double pixelsPerDay)
        {
            _horizontalCanvas.Children.Clear();

            const int minimumPixelsPerTick = 60;
            const double maximumTicks = 10;
            var pixelsPerTick = Math.Max(_horizontalCanvas.ActualWidth / maximumTicks, minimumPixelsPerTick);
            var periodsBetweenHorizontalTicks = pixelsPerTick / pixelsPerDay;
            for (DateTime i = firstDay; i < lastDay; i = i.AddDays(periodsBetweenHorizontalTicks))
            {
                DrawHorizontalTick(pixelsPerDay, i, firstDay);
            }
            var separator = new Line();
            separator.Y1 = 0;
            separator.X1 = 0;
            separator.X2 = _horizontalCanvas.ActualWidth;
            separator.Y2 = 0;
            separator.Stroke = Brushes.DarkBlue;
            _horizontalCanvas.Children.Add(separator);
        }

        private void DrawHorizontalTick(double pixelsPerDay, DateTime tickDay, DateTime firstDay)
        {
            var line = new Line();
            line.Y1 = 0;
            line.Y2 = 5;
            line.X1 = (tickDay - firstDay).Days*pixelsPerDay;
            line.X2 = (tickDay - firstDay).Days*pixelsPerDay;
            line.Stroke = Brushes.Black;
            _horizontalCanvas.Children.Add(line);
            var textBlock = new TextBlock();
            textBlock.Text = tickDay.ToShortDateString();
            textBlock.Foreground = Brushes.Black;
            Canvas.SetTop(textBlock, 7);
            Canvas.SetLeft(textBlock, line.X1 - 25);

            _horizontalCanvas.Children.Add(textBlock);
        }

        private void DrawVerticalAxis(double highestHigh, double lowestLow, double pixelsPerDollar)
        {
            _verticalCanvas.Children.Clear();

            const int minimumPixelsPerTick = 30;
            const double maximumTicks = 10;
            var pixelsPerTick = Math.Max(_verticalCanvas.ActualHeight/maximumTicks, minimumPixelsPerTick);
            var dollarsBetweenVerticalTicks = pixelsPerTick/pixelsPerDollar;
            for (double i = 0; i < Math.Round(highestHigh - lowestLow, 0); i += dollarsBetweenVerticalTicks)
            {
                DrawVerticalTick(pixelsPerDollar, i, highestHigh);
            }
            var separator = new Line();
            separator.X1 = _verticalCanvas.ActualWidth;
            separator.Y1 = 0;
            separator.X2 = _verticalCanvas.ActualWidth;
            separator.Y2 = _verticalCanvas.ActualHeight;
            separator.Stroke = Brushes.DarkBlue;
            _verticalCanvas.Children.Add(separator);
        }

        private void DrawVerticalTick(double pixelsPerDollar, double offsetPrice, double highestHigh)
        {
            var line = new Line();
            line.Y1 = offsetPrice*pixelsPerDollar;
            line.Y2 = offsetPrice*pixelsPerDollar;
            line.X1 = 40;
            line.X2 = 45;
            line.Stroke = Brushes.Black;
            var textBlock = new TextBlock();
            textBlock.Text = (highestHigh - offsetPrice).ToString("C");
            textBlock.Foreground = Brushes.Black;
            Canvas.SetLeft(textBlock, 5);
            Canvas.SetTop(textBlock, offsetPrice*pixelsPerDollar - (6));

            _verticalCanvas.Children.Add(textBlock);
            _verticalCanvas.Children.Add(line);
        }

        private void DrawCanvas(double maxYdollar, double minYdollar, double pixelsPerDollar, double pixelsPerDay)
        {
            _canvas.Children.Clear();

            var minX = _pricePeriods.First().Head;

            foreach (var pricePeriod in _pricePeriods)
            {
                var line = new Line();
                line.X1 = ((pricePeriod.Head - minX).Days*pixelsPerDay);
                line.Y1 = _canvas.ActualHeight - ((double) pricePeriod.Low - minYdollar)*pixelsPerDollar;
                line.X2 = line.X1;
                line.Y2 = (maxYdollar - (double) pricePeriod.High)*pixelsPerDollar;
                line.Stroke = Brushes.Blue;

                _canvas.Children.Add(line);
            }
        }
    }
}
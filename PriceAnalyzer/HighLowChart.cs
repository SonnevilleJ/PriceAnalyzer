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

        public HighLowChart()
        {
            var column1 = new ColumnDefinition();
            column1.Width = new GridLength(45);
            ColumnDefinitions.Add(column1);
            ColumnDefinitions.Add(new ColumnDefinition());
            Grid.SetColumn(_verticalCanvas, 0);
            Grid.SetColumn(_canvas, 1);


            Children.Add(_canvas);
            Children.Add(_verticalCanvas);
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
            var pixelsPerDollar = (ActualHeight/dollarRange);
            DrawCanvas(highestHigh, lowestLow, pixelsPerDollar);
            VerticalAxis(highestHigh, lowestLow, pixelsPerDollar);
        }

        private void VerticalAxis(double highestHigh, double lowestLow, double pixelsPerDollar)
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

        private void DrawCanvas(double maxYdollar, double minYdollar, double pixelsPerDollar)
        {
            _canvas.Children.Clear();

            var minX = _pricePeriods.First().Head;
            var maxX = _pricePeriods.Last().Tail;

            foreach (var pricePeriod in _pricePeriods)
            {
                var line = new Line();
                line.X1 = ((pricePeriod.Head - minX).Days*5) + 5;
                line.Y1 = ActualHeight - ((double) pricePeriod.Low - minYdollar)*pixelsPerDollar;
                line.X2 = line.X1;
                line.Y2 = (maxYdollar - (double) pricePeriod.High)*pixelsPerDollar;
                line.Stroke = Brushes.Blue;

                _canvas.Children.Add(line);
            }
        }
    }
}
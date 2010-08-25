using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Creates chart images from price data.
    /// </summary>
    public class ChartBuilder
    {
        private IPriceTuple _tuple;

        public ChartBuilder(IPriceTuple tuple)
        {
            _tuple = tuple;
        }

        public ImageSource GenerateCandlestickChart()
        {
            DrawingImage chart = new DrawingImage();

            GeometryDrawing drawing = new GeometryDrawing();
            drawing.Brush = Brushes.White;
            drawing.Geometry = new RectangleGeometry(new Rect(0, 0, 500, 300));






            return chart;
        }

        private void CreateCandleGroups(out GeometryGroup red, out GeometryGroup black)
        {
            // TODO: Make these values adjustable based on chart size and tuple high/low.
            int margin = 5;
            int candleWidth = 10;
            int yMin = 0;
            int yMax = 100;
            red = new GeometryGroup();
            black = new GeometryGroup();

            IPricePeriod[] p = _tuple.Periods;
            for (int i = 0, cursor = 0; i < p.Length; i++, cursor += candleWidth + margin)
            {
                if (p[i].Close >= p[i].Open)
                {
                    // black candle
                    black.Children.Add(CreateCandle(p[i], margin, candleWidth, cursor, yMin, yMax));
                }
                else
                {
                    // red candle
                    red.Children.Add(CreateCandle(p[i], margin, candleWidth, cursor, yMin, yMax));
                }
            }
        }

        private RectangleGeometry CreateCandle(IPricePeriod pricePeriod, int margin, int candleWidth, int x, int yMin, int yMax)
        {
            throw new NotImplementedException();
        }
    }
}

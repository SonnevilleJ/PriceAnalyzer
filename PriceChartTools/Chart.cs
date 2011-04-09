using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LineCharts;

namespace Sonneville.PriceChartTools
{
    public class Chart : UserControl
    {
        public class Legend
        {
            internal Legend()
            {
                IsLegend = false;
                HasBorder = true;
                Position = CardinalDirection.NorthWest;
            }

            /// <summary>
            /// The location of the Legend within its parent chart.
            /// </summary>
            public CardinalDirection Position { get; set; }
            public Canvas Canvas { get; set; }
            public bool IsLegend { get; set; }

            /// <summary>
            /// Gets a value indicating if the Legend should have a border.
            /// </summary>
            public bool HasBorder { get; set; }
            


            public void AddLegend(Canvas canvas, DataCollection dc)
            {
                if (dc.DataList.Count < 1 || !IsLegend) return;

                string[] legendLabels = new string[dc.DataList.Count];
                for (int n = 0; n < dc.DataList.Count; n++)
                {
                    legendLabels[n] = dc.DataList[n].Ticker;
                }

                double legendWidth = 0;
                Size size = new Size(0, 0);
                
                // set legendWidth to length of widest label
                foreach (string t in legendLabels)
                {
                    TextBlock textBlock = new TextBlock {Text = t};
                    textBlock.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                    size = textBlock.DesiredSize;
                    if (legendWidth < size.Width)
                    {
                        legendWidth = size.Width;
                    }
                }

                legendWidth += 50;
                Canvas.Width = legendWidth + 5;
                double legendHeight = 17 * dc.DataList.Count;
                const double sx = 6;
                const double sy = 0;
                double textHeight = size.Height;
                const double lineLength = 34;

                Rectangle legendRect = new Rectangle
                                           {
                                               Stroke = BorderColor,
                                               Fill = FillColor,
                                               Width = legendWidth,
                                               Height = legendHeight
                                           };

                if (IsLegend && HasBorder)
                {
                    Canvas.Children.Add(legendRect);
                }
                Panel.SetZIndex(Canvas, 10);

                // for each legend entry
                for (int n = 1; n < dc.DataList.Count; n++)
                {
                    DataSeries ds = dc.DataList[n];
                    const double xText = 2*sx + lineLength;
                    double yText = n*sy + (2*n - 1)*textHeight/2;

                    Line line = new Line {X1 = sx, Y1 = yText, X2 = sx + lineLength, Y2 = yText};
                    AddLinePattern(line, ds);
                    Canvas.Children.Add(line);
                    ds.Symbols.AddSymbol(Canvas, new Point(0.5*(line.X2 - line.X1 + ds.Symbols.SymbolSize) + 1, line.Y1));

                    TextBlock textBlock = new TextBlock {Text = ds.Ticker};
                    Canvas.Children.Add(textBlock);
                    Canvas.SetTop(textBlock, yText - size.Height/2);
                    Canvas.SetLeft(textBlock, xText);
                }

                Canvas.Width = legendRect.Width;
                Canvas.Height = legendRect.Height;

                const double offSet = 7.0;
                PositionLegend(legendRect, offSet, canvas);
            }

            private void PositionLegend(Rectangle legend, double offSet, Canvas canvas)
            {
                switch (Position)
                {
                    case CardinalDirection.East:
                        Canvas.SetRight(Canvas, offSet);
                        Canvas.SetTop(Canvas, canvas.Height / 2 - legend.Height / 2);
                        break;
                    case CardinalDirection.NorthEast:
                        Canvas.SetTop(Canvas, offSet);
                        Canvas.SetRight(Canvas, offSet);
                        break;
                    case CardinalDirection.North:
                        Canvas.SetTop(Canvas, offSet);
                        Canvas.SetLeft(Canvas, canvas.Width / 2 - legend.Width / 2);
                        break;
                    case CardinalDirection.NorthWest:
                        Canvas.SetTop(Canvas, offSet);
                        Canvas.SetLeft(Canvas, offSet);
                        break;
                    case CardinalDirection.West:
                        Canvas.SetTop(Canvas, canvas.Height / 2 - legend.Height / 2);
                        Canvas.SetLeft(Canvas, offSet);
                        break;
                    case CardinalDirection.SouthWest:
                        Canvas.SetBottom(Canvas, offSet);
                        Canvas.SetLeft(Canvas, offSet);
                        break;
                    case CardinalDirection.South:
                        Canvas.SetBottom(Canvas, offSet);
                        Canvas.SetLeft(Canvas, canvas.Width / 2 - legend.Width / 2);
                        break;
                    case CardinalDirection.SouthEast:
                        Canvas.SetBottom(Canvas, offSet);
                        Canvas.SetRight(Canvas, offSet);
                        break;
                }
            }

            /// <summary>
            /// Gets the fill color for the Legend.
            /// </summary>
            private static SolidColorBrush FillColor
            {
                get { return Brushes.White; }
            }

            /// <summary>
            /// Gets the stroke color for the Legend.
            /// </summary>
            private static SolidColorBrush BorderColor
            {
                get { return Brushes.Black; }
            }

            private static void AddLinePattern(Line line, DataSeries ds)
            {
                line.Stroke = ds.LineColor;
                line.StrokeThickness = ds.LineThickness;

                switch (ds.LinePattern)
                {
                    case LinePattern.Dash:
                        line.StrokeDashArray = new DoubleCollection(new double[] { 4, 3 });
                        break;
                    case LinePattern.Dot:
                        line.StrokeDashArray = new DoubleCollection(new double[] { 1, 2 });
                        break;
                    case LinePattern.DashDot:
                        line.StrokeDashArray = new DoubleCollection(new double[] { 4, 2, 1, 2 });
                        break;
                    case LinePattern.None:
                        line.Stroke = Brushes.Transparent;
                        break;
                }
            }
        }
    }
}

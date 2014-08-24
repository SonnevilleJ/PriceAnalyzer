using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    /// <summary>
    /// Interaction logic for Chart.xaml
    /// </summary>
    public partial class Chart : UserControl
    {
        private IEnumerable<KeyValuePair<IList<IPricePeriod>, IRenderer>> Dictionary { get; set; }

        public Chart()
        {
            InitializeComponent();
            SizeChanged += (sender, args) => Redraw();
        }

        public void Draw(IEnumerable<KeyValuePair<IList<IPricePeriod>, IRenderer>> dictionary)
        {
            Dictionary = dictionary;
            Horizontal.PricePeriods = Dictionary.First().Key;
            
            Redraw();
        }

        private void Redraw()
        {
            if (Dictionary != null)
            {
                Clear();
                DrawAllDatasets(Dictionary);
            }
        }

        private void Clear()
        {
            _canvas.Children.Clear();
        }

        private void DrawAllDatasets(IEnumerable<KeyValuePair<IList<IPricePeriod>, IRenderer>> dictionary)
        {
            foreach (var kvp in dictionary)
            {
                DrawSingleDataset(kvp.Key, kvp.Value);
            }
        }

        private void DrawSingleDataset(IList<IPricePeriod> pricePeriods, IRenderer renderer)
        {
            if (Equals0(_canvas.ActualHeight) || Equals0(_canvas.ActualWidth))
            {
                return;
            }

            var highestHigh = (double) pricePeriods.Max(pricePeriod => pricePeriod.High);
            var lowestLow = (double) pricePeriods.Min(pricePeriod => pricePeriod.Low);

            var dollarRange = (highestHigh - lowestLow);
            var pixelsPerDollar = (_canvas.ActualHeight/dollarRange);
            Vertical.DrawVerticalAxis(highestHigh, lowestLow, pixelsPerDollar);

            var pixelsPerDay = _canvas.ActualWidth/pricePeriods.Count;
            Horizontal.DrawHorizontalAxis(pixelsPerDay);
            
            renderer.DrawCanvas(highestHigh, lowestLow, pixelsPerDollar, pixelsPerDay, _canvas, pricePeriods);
        }

        private bool Equals0(double value)
        {
            return Math.Abs(value) < 0.1;
        }
    }
}
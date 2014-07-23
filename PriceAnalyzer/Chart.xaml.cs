using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    /// <summary>
    /// Interaction logic for Chart.xaml
    /// </summary>
    public partial class Chart : UserControl
    {
        private IRenderer _renderer;
        private readonly RendererFactory _rendererFactory = new RendererFactory();

        public Chart()
        {
            InitializeComponent();
            SizeChanged += delegate { if (PricePeriods != null) DrawChart(PricePeriods, Renderer); };
        }

        public IList<IPricePeriod> PricePeriods { get; private set; }

        private IRenderer Renderer
        {
            get { return _renderer ?? _rendererFactory.CreateNewRenderer(); }
        }

        public void DrawPricePeriods(IEnumerable<KeyValuePair<IList<IPricePeriod>, IRenderer>> dictionary)
        {
            _canvas.Children.Clear();
            foreach (var kvp in dictionary)
            {
                Horizontal.PricePeriods = kvp.Key;
                DrawChart(kvp.Key, kvp.Value);
            }
        }

        public void DrawPricePeriods(IList<IPricePeriod> pricePeriods, IRenderer renderer)
        {
            _renderer = renderer;
            PricePeriods = pricePeriods;
            Horizontal.PricePeriods = pricePeriods;
            
            _canvas.Children.Clear();
            
            DrawChart(PricePeriods, Renderer);
        }

        private void DrawChart(IList<IPricePeriod> pricePeriods, IRenderer renderer)
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
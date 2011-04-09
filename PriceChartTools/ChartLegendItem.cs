using System.Windows;
using System.Windows.Media;

namespace Sonneville.PriceChartTools
{
    /// <summary>
    /// Represents an entry for a ChartLegend.
    /// </summary>
    public class ChartLegendItem : FrameworkElement
    {
        public ChartLegendItem()
        {
        }
        
        public ChartLegendItem(string label, LinePattern linePattern = LinePattern.Solid)
            : this(label, linePattern, Brushes.Black)
        {
        }

        public ChartLegendItem(string label, LinePattern linePattern, Brush color)
        {
            Label = label;
            LinePattern = linePattern;
            Brush = color;
        }

        public string Label { get; set; }
        public LinePattern LinePattern { get; set; }
        public Brush Brush { get; set; }
    }
}

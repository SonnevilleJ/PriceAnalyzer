using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Sonneville.PriceChartTools
{
    public class ChartCanvas : Canvas
    {
        static ChartCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChartCanvas), new FrameworkPropertyMetadata(typeof(ChartCanvas)));
        }

        protected override Size MeasureOverride(Size constraint)
        {
            double bottomMost = 0;
            double rightMost = 0;

            foreach (var child in Children.OfType<FrameworkElement>())
            {
                child.Measure(constraint);
                bottomMost = Math.Max(bottomMost, GetTop(child) + child.DesiredSize.Height);
                rightMost = Math.Max(rightMost, GetLeft(child) + child.DesiredSize.Width);
            }

            if (double.IsNaN(bottomMost) || double.IsInfinity(bottomMost))
            {
                bottomMost = 0;
            }
            if (double.IsNaN(rightMost) || double.IsInfinity(rightMost))
            {
                rightMost = 0;
            }

            return new Size(rightMost, bottomMost);
        }
    }
}

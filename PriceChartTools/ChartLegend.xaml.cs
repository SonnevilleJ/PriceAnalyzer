using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sonneville.PriceChartTools
{
    /// <summary>
    /// Interaction logic for ChartLegend.xaml
    /// </summary>
    [ContentProperty("Items")]
    public partial class ChartLegend : ItemsControl
    {
        private readonly Thickness _itemMargin = new Thickness(5, 2.5, 5, 2.5);

        /// <summary>
        /// Constructs a new ChartLegend.
        /// </summary>
        public ChartLegend()
        {
            InitializeComponent();

            Position = CardinalDirection.NorthEast;
            Foreground = Brushes.White;
            BorderBrush = Brushes.Black;
        }
        
        /// <summary>
        /// Gets or sets the relative position of this Legend within a container (such as a chart).
        /// </summary>
        public CardinalDirection Position { get; set; }
        
        public List<ChartLegendItem> Items
        {
            get { return (List<ChartLegendItem>) GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(List<ChartLegendItem>), typeof(ChartLegendItem),
            new FrameworkPropertyMetadata(
                new ChartLegendItem{Brush= Brushes.Blue, LinePattern = LinePattern.Solid, Label = "Test"},
                FrameworkPropertyMetadataOptions.AffectsRender));

        public Thickness ItemMargin
        {
            get { return _itemMargin; }
        }

        ///// <summary>
        ///// Adds a new name/line pair to the Legend.
        ///// </summary>
        ///// <param name="name">The name to display.</param>
        ///// <param name="linePattern">The LinePattern to display.</param>
        //public void AddLegend(string name, LinePattern linePattern)
        //{
        //    if(Legends.ContainsKey(name))
        //    {
        //        throw new InvalidOperationException();
        //    }

        //    _legends.Add(name, linePattern);

        //    TextStack.Children.Add(new TextBlock {Margin = ItemMargin, Text = name});
        //    var line = new Line {Margin = ItemMargin, Width = 10};
        //    AddLinePattern(line, linePattern);
        //    LineStack.Children.Add(line);
        //}

        /// <summary>
        /// Draws the name/line pairs.
        /// </summary>
        public void DrawLegend()
        {
            
        }

        private static void AddLinePattern(Line line, LinePattern linePattern)
        {
            switch (linePattern)
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

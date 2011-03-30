using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sonneville.PriceChartTools
{
    /// <summary>
    /// Interaction logic for Axis.xaml
    /// </summary>
    public partial class Axis : UserControl
    {
        private DateTime _head;
        private DateTime _tail;

        public Axis()
        {
            InitializeComponent();
        }

        public void DebugMe()
        {
            grid.Children.Clear();

            Head = new DateTime(2011, 1, 1);
            Tail = new DateTime(2011, 12, 31);

            for (int i = 1; i <= 12; i++)
            {
                DateTime date = new DateTime(2011, i, 1);
                Label label = new Label
                                  {
                                      Content = GetMonth(date),
                                      Margin = new Thickness(GetX(date), 0, 0, 0),
                                      Padding = new Thickness(0),
                                      HorizontalAlignment = HorizontalAlignment.Left
                                  };
                grid.Children.Add(label);
            }
        }

        public DateTime Head
        {
            get { return _head; }
            set { _head = value; }
        }

        public DateTime Tail
        {
            get { return _tail; }
            set { _tail = value; }
        }

        public TimeSpan PeriodLength { get; set; }

        public void Draw()
        {
            double periodCount = (Tail - Head).Ticks / (double) PeriodLength.Ticks;
            double periodWidth = Width / periodCount;

            for (int i = 0; i < periodCount; i++)
            {
                Label label = new Label();
                label.Content = (Head + new TimeSpan(PeriodLength.Ticks*i)).ToString();
            }
        }

        private double GetX(DateTime dateTime)
        {
            long ticksToDate = dateTime.Ticks - Head.Ticks;
            long ticksToTail = Tail.Ticks - Head.Ticks;
            return (ticksToDate / (double) ticksToTail) * Width;
        }

        private static char GetMonth(DateTime dateTime)
        {
            switch (dateTime.Month)
            {
                case 1:
                    return 'J';
                case 2:
                    return 'F';
                case 3:
                    return 'M';
                case 4:
                    return 'A';
                case 5:
                    return 'M';
                case 6:
                    return 'J';
                case 7:
                    return 'J';
                case 8:
                    return 'A';
                case 9:
                    return 'S';
                case 10:
                    return 'O';
                case 11:
                    return 'N';
                case 12:
                    return 'D';
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(e.WidthChanged)
            {
                DebugMe();
            }
        }
    }
}

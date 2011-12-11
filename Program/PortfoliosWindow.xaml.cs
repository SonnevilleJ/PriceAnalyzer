using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Sonneville.PriceTools;

namespace Sonneville.PriceAnalyzer
{
    /// <summary>
    /// Interaction logic for PortfoliosWindow.xaml
    /// </summary>
    public partial class PortfoliosWindow : Window
    {
        private readonly Container _container = new Container();

        public PortfoliosWindow()
        {
            InitializeComponent();

            LoadPortfolios();
        }

        private ObservableCollection<Portfolio> Portfolios { get; set; }

        private void LoadPortfolios()
        {
            throw new NotImplementedException();
            //Portfolios = new ObservableCollection<Portfolio>(_container.Portfolios);
            //DataContext = Portfolios;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Portfolios.Add(new Portfolio("Test"));

        }
    }
}

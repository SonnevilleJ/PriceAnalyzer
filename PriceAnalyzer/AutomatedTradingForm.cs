using System;
using System.Windows.Forms;
using Sonneville.PriceTools.AutomatedTrading;
using Sonneville.PriceTools.AutomatedTrading.Implementation;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public partial class AutomatedTradingForm : Form
    {
        private readonly AutomatedTradingViewModel _viewModel;

        public AutomatedTradingForm(AutomatedTradingViewModel viewModel, IPortfolioFactory portfolioFactory)
        {
            InitializeComponent();
            _viewModel = viewModel;
            Portfolio = portfolioFactory.ConstructPortfolio("cash", new DateTime(2014, 1, 1), 1000000);
            startTimePicker.Value = DateTime.Now.AddHours(-1);
        }

        public IPortfolio Portfolio { get; private set; }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ProcessButton_Click(object sender, EventArgs e)
        {
            var tickers = stockTextBox.Text.Split(new[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries);
            _viewModel.Run(Portfolio, startTimePicker.Value, endTimePicker.Value, tickers);
            Close();
        }

        private void startTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DateErrorCheck(endTimePicker, startTimePicker);
        }

        private void endTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DateErrorCheck(endTimePicker, startTimePicker);
        }

        private void DateErrorCheck(DateTimePicker laterPicker, DateTimePicker earlierPicker)
        {
            if (laterPicker.Value <= earlierPicker.Value)
            {
                MessageBox.Show("End date must be after start date.", "User input Error", MessageBoxButtons.OK);
                earlierPicker.Value = laterPicker.Value.AddHours(-1);
            }
        }
    }
}

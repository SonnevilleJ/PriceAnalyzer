using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;
using Sonneville.PriceTools.AutomatedTrading;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public partial class AutomatedTradingForm : Form
    {
        private AutomatedTradingViewModel _viewModel;

        public AutomatedTradingForm()
        {
            InitializeComponent();
            _viewModel = new AutomatedTradingViewModel(new TradingProcess(new AnalysisEngine(new SecurityBasketCalculator()), new PriceSeriesFactory(), new SimulatedBrokerage()));
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ProcessButton_Click(object sender, EventArgs e)
        {
            var portfolio = new PortfolioFactory().ConstructPortfolio("cash");
            var tickers = stockTextBox.Text.Split(new[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries);
            _viewModel.Run(portfolio, startTimePicker.Value, endTimePicker.Value, tickers);
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
            if (laterPicker.Value < earlierPicker.Value)
            {
                MessageBox.Show("End date must be after start date.", "User input Error", MessageBoxButtons.OK);
                earlierPicker.Value = (laterPicker.Value);
            }
        }
    }
}

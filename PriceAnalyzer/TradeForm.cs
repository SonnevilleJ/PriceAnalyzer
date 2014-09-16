using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sonneville.PriceTools.AutomatedTrading;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public partial class TradeForm : Form
    {
        private readonly TradeViewModel _viewModel;
        private IBrokerage _brokerage;

        public TradeForm(OrderType orderType, IBrokerage brokerage)
        {
            InitializeComponent();
            Text = orderType + " Shares";
            _brokerage = brokerage;
            _viewModel = new TradeViewModel(_brokerage);
            _viewModel.OrderType = orderType;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            _viewModel.Ticker = stockTextBox.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            _viewModel.Volume = decimal.Parse(volumeTextBox.Text);
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            if (!_viewModel.ValidateVolume(volumeTextBox.Text))
            {
                e.Cancel = true;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            _viewModel.SharePrice = decimal.Parse(priceTextBox.Text);
        }

        private void textBox3_Validating(object sender, CancelEventArgs e)
        {
            if (!_viewModel.ValidatePrice(priceTextBox.Text))
            {
                e.Cancel = true;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            _viewModel.ExpirationType = Expiration.NoExpiration;
            dateTimePicker.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            _viewModel.ExpirationType = Expiration.NextMarketClose;
            dateTimePicker.Enabled = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            _viewModel.ExpirationType = Expiration.SpecificDateTime;
            dateTimePicker.Enabled = true;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            _viewModel.ExpirationDateTime = dateTimePicker.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _viewModel.Submit();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

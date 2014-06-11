using System;
using System.Windows.Forms;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public partial class DataEntryForm : Form
    {
        public DataEntryForm()
        {
            InitializeComponent();

            startDateTimePicker.Value = DateTime.Now.AddMonths(-1);
        }

        public string Ticker { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        private void downloadButton_Click(object sender, EventArgs e)
        {
            StartDateTime = startDateTimePicker.Value;
            EndDateTime = endDateTimePicker.Value;
            Ticker = TickerTextBox.Text;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

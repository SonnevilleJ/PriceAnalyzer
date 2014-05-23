using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public partial class GenericForm : Form
    {
        private readonly PriceDataManager _priceDataManager = new PriceDataManager();

        protected GenericForm()
        {
            InitializeComponent();
            
            startDateTimePicker.Value = DateTime.Now.AddMonths(-1);
        }

        private void ImportClick(object sender, EventArgs e)
        {
            var dialogResult = openFileDialog1.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                var fullFileName = openFileDialog1.FileName;
                var pricePeriods = _priceDataManager.ParseCsvFile(fullFileName);
                var fileName = new FileInfo(fullFileName).Name;
                var tabName = fileName.Substring(0, fileName.IndexOf("."));
                DisplayContent(pricePeriods, tabName);
            }
        }

        protected virtual void DisplayContent(IList<IPricePeriod> pricePeriods, string ticker)
        {
        }

        private void CloseClick(object sender, EventArgs e)
        {
            Close();
        }

        private void downloadButton_Click(object sender, EventArgs e)
        {
            downloadButton.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            var ticker = TickerTextBox.Text;
            DisplayContent(_priceDataManager.DownloadPricePeriods(ticker, startDateTimePicker.Value, endDateTimePicker.Value),ticker);
            
            downloadButton.Enabled = true;
            this.Cursor = Cursors.Default;
        }
    }
}

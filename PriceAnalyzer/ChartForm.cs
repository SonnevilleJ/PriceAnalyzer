﻿using System;
using System.Windows.Forms;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public partial class ChartForm : Form
    {
        private readonly PriceDataManager _priceDataManager = new PriceDataManager();

        public ChartForm()
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
                
                highLowChart1.DrawPricePeriods(pricePeriods);
            }
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
            var pricePeriods = _priceDataManager.DownloadPricePeriods(ticker, startDateTimePicker.Value, endDateTimePicker.Value);
            highLowChart1.DrawPricePeriods(pricePeriods);

            downloadButton.Enabled = true;
            this.Cursor = Cursors.Default;
        }
    }
}

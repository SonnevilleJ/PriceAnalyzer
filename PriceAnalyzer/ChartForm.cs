using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

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
                var fileName = new FileInfo(fullFileName).Name;
                var tabName = fileName.Substring(0, fileName.IndexOf("."));
                DisplayContent(pricePeriods, tabName);
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
            DisplayContent(_priceDataManager.DownloadPricePeriods(ticker, startDateTimePicker.Value, endDateTimePicker.Value), ticker);

            downloadButton.Enabled = true;
            this.Cursor = Cursors.Default;
        }

        private void DisplayContent(IList<IPricePeriod> pricePeriods, string ticker)
        {
            var currentTab = this.tabControl1.SelectedTab;
            var currentElementHost = (ElementHost)currentTab.Controls[0];
            var currentChart = (HighLowChart)currentElementHost.Child;

            currentChart.DrawPricePeriods(pricePeriods);
            currentTab.Text = ticker;
        }

        private void addTabButton_Click(object sender, System.EventArgs e)
        {
            var newTab = new TabPage(tabControl1.TabCount.ToString());
            var newElementHost = new ElementHost();
            var newChart = new HighLowChart();

            this.tabControl1.TabPages.Add(newTab);
            newTab.Controls.Add(newElementHost);
            newElementHost.Child = newChart;
            newElementHost.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            newElementHost.Dock = DockStyle.Fill;
            newTab.UseVisualStyleBackColor = true;
            this.tabControl1.SelectedTab = newTab;

        }
    }
}

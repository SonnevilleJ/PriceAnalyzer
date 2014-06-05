using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public partial class MainForm : Form
    {
        private readonly PriceDataManager _priceDataManager = new PriceDataManager();

        public MainForm()
        {
            InitializeComponent();

            startDateTimePicker.Value = DateTime.Now.AddMonths(-1);
        }

        private void ImportCSV()
        {
            var dialogResult = openFileDialog1.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                var fullFileName = openFileDialog1.FileName;
                var pricePeriods = _priceDataManager.ParseCsvFile(fullFileName);
                var fileName = new FileInfo(fullFileName).Name;
                var ticker = fileName.Substring(0, fileName.IndexOf("."));
                DisplayData(pricePeriods, ticker);
            }
        }

        private void downloadButton_Click(object sender, EventArgs e)
        {
            downloadButton.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            var ticker = TickerTextBox.Text;
            var pricePeriods = _priceDataManager.DownloadPricePeriods(ticker, startDateTimePicker.Value, endDateTimePicker.Value);
            DisplayData(pricePeriods, ticker);

            downloadButton.Enabled = true;
            this.Cursor = Cursors.Default;
        }

        private void DisplayData(IList<IPricePeriod> pricePeriods, string ticker)
        {
            var currentTab = tabControl1.SelectedTab;
            currentTab.Text = ticker;

            if (tabControl1.SelectedTab.Controls[0] is ElementHost)
            {
                DisplayChart(pricePeriods, currentTab);
            }
            else
            {
                DisplayTable(pricePeriods, currentTab);
            }
        }

        private void DisplayTable(IList<IPricePeriod> pricePeriods, Control currentTab)
        {
            var dataGridView = (DataGridView)currentTab.Controls[0];
            for (int i = 0; i < pricePeriods.Count; i++)
            {
                var pricePeriod = pricePeriods[i];

                dataGridView.Rows.Add();
                var row = dataGridView.Rows[i];
                row.Cells["Date"].Value = pricePeriod.Head.Date.ToShortDateString();
                row.Cells["Closing price"].Value = pricePeriod.Close;
                row.Cells["High"].Value = pricePeriod.High;
                row.Cells["Low"].Value = pricePeriod.Low;
                row.Cells["Volume"].Value = pricePeriod.Volume;
                row.Cells["Open"].Value = pricePeriod.Open;
            }
        }

        private void DisplayChart(IList<IPricePeriod> pricePeriods, Control currentTab)
        {
            var currentElementHost = (ElementHost)currentTab.Controls[0];
            var currentChart = (HighLowChart)currentElementHost.Child;

            currentChart.DrawPricePeriods(pricePeriods);
        }

        private void AddChartTab()
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

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void importCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportCSV();
        }

        private void chartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddChartTab();
        }

        private void tableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tabPage = new TabPage(tabControl1.TabCount.ToString());
            tabControl1.TabPages.Add(tabPage);
            var dataGridView = new DataGridView();
            dataGridView.Dock = DockStyle.Fill;
            tabPage.Controls.Add(dataGridView);

            dataGridView.Columns.Add("Date", "Date");
            dataGridView.Columns.Add("Open", "Open");
            dataGridView.Columns.Add("Closing price", "Closing price");
            dataGridView.Columns.Add("Low", "Low");
            dataGridView.Columns.Add("High", "High");
            dataGridView.Columns.Add("Volume", "Volume");

            this.tabControl1.SelectedTab = tabPage;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using Sonneville.PriceTools.AutomatedTrading;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Fidelity;
using Sonneville.PriceTools.Google;
using Sonneville.PriceTools.Implementation;
using Sonneville.PriceTools.Yahoo;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public partial class MainForm : Form
    {
        private readonly PriceDataManager _priceDataManager = new PriceDataManager();

        public MainForm()
        {
            InitializeComponent();
        }

        private void DownloadPriceData(string ticker, DateTime startDateTime, DateTime endDateTime)
        {
            this.Cursor = Cursors.WaitCursor;

            var pricePeriods = _priceDataManager.DownloadPricePeriods(ticker, startDateTime, endDateTime);
            DisplayDataInCurrentTab(pricePeriods, ticker);

            this.Cursor = Cursors.Default;
        }

        private void ImportCsv()
        {
            var dialogResult = openFileDialog1.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                var fullFileName = openFileDialog1.FileName;
                var pricePeriods = _priceDataManager.ParseCsvFile(fullFileName);
                var fileName = new FileInfo(fullFileName).Name;
                var ticker = fileName.Substring(0, fileName.IndexOf("."));
                DisplayDataInCurrentTab(pricePeriods, ticker);
            }
        }

        private void DisplayDataInCurrentTab(IList<IPricePeriod> pricePeriods, string ticker)
        {
            var currentTab = tabControl1.SelectedTab;
            currentTab.Text = ticker;

            if (currentTab.Controls[0] is ElementHost)
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

        private void DownloadStockData()
        {
            var dataEntryForm = new DataEntryForm();
            var dialogResult = dataEntryForm.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                var ticker = dataEntryForm.Ticker;
                var startDate = dataEntryForm.StartDateTime;
                var endDate = dataEntryForm.EndDateTime;

                DownloadPriceData(ticker, startDate, endDate);
            }
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

        private void AddTableTab()
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

        private void downloadStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DownloadStockData();
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void importCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportCsv();
        }

        private void chartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddChartTab();
        }

        private void tableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTableTab();
        }

        private void importPortfolioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tempPortfolioFactory = new PortfolioFactory();
            var portfolioDialog = openFileDialog1.ShowDialog();

            if (portfolioDialog == DialogResult.OK)
            {
                var fullFileName = openFileDialog1.FileName;

                var transactionHistoryCsvFile = new FidelityTransactionHistoryCsvFile(File.Open(fullFileName, FileMode.Open));
                
                var cashTicker = transactionHistoryCsvFile.Transactions
                    .Where(transaction => transaction is ShareTransaction)
                    .Cast<ShareTransaction>()
                    .Select(transaction => transaction.Ticker)
                    .First(ticker => ticker.EndsWith("XX"));
                
                var portfolio = tempPortfolioFactory.ConstructPortfolio(cashTicker, transactionHistoryCsvFile.Transactions);
                var priceSeries = tempPortfolioFactory.ConstructPriceSeries(portfolio, new PriceDataProvider(new GooglePriceHistoryQueryUrlBuilder(), new GooglePriceHistoryCsvFileFactory()));

                DisplayDataInCurrentTab(priceSeries.PricePeriods.ToList(), "Portfolio");
            }
        }
    }
}

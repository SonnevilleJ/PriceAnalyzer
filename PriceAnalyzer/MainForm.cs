﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using Sonneville.PriceTools.AutomatedTrading;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Fidelity;
using Sonneville.PriceTools.Google;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public partial class MainForm : Form
    {
        private readonly ChartFactory _chartFactory = new ChartFactory();
        private readonly MainFormViewModel _viewModel = new MainFormViewModel();
        private readonly DataEntryForm _dataEntryForm = new DataEntryForm();

        public MainForm()
        {
            InitializeComponent();
            var defaultChartStyle = (ChartStyles)Enum.Parse(typeof(ChartStyles), Settings.Default.ChartStyle);

            SetDefaultChartStyle(defaultChartStyle);
            this.candleStickToolStripMenuItem.Checked = defaultChartStyle == ChartStyles.CandlestickChart;
            this.oHLCToolStripMenuItem.Checked = defaultChartStyle == ChartStyles.OpenHighLowClose;
        }

        private void SetDefaultChartStyle(ChartStyles defaultChartStyle)
        {
            view_ChartStyle_CandleStick.Checked = defaultChartStyle == ChartStyles.CandlestickChart;
            view_ChartStyle_Ohlc.Checked = defaultChartStyle == ChartStyles.OpenHighLowClose;
            Settings.Default.ChartStyle = defaultChartStyle.ToString();
            Settings.Default.Save();
        }

        private void DownloadPriceData(string ticker, DateTime startDateTime, DateTime endDateTime)
        {
            var pricePeriods = _viewModel.Download(ticker, startDateTime, endDateTime);
            DisplayDataInCurrentTab(pricePeriods, ticker);
        }

        private void ImportCsv()
        {
            var dialogResult = openFileDialog1.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                var fullFileName = openFileDialog1.FileName;
                var pricePeriods = _viewModel.OpenFile(fullFileName);
                DisplayDataInCurrentTab(pricePeriods, _viewModel.Ticker);
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
            var currentChart = (ChartBase)currentElementHost.Child;

            currentChart.DrawPricePeriods(pricePeriods);
        }

        private void DownloadStockData(DataEntryForm dataEntryForm)
        {
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
            var newChart = _chartFactory.CreateNewChart();

            this.tabControl1.TabPages.Add(newTab);
            newTab.Controls.Add(newElementHost);
            newElementHost.Child = (UIElement) newChart;
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
            this.Cursor = Cursors.WaitCursor;

            DownloadStockData(_dataEntryForm);
            
            this.Cursor = Cursors.Default;
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

        private void view_ChartStyle_CandleStick_Click(object sender, EventArgs e)
        {
            SetDefaultChartStyle(ChartStyles.CandlestickChart);
        }

        private void view_ChartStyle_Ohlc_Click(object sender, EventArgs e)
        {
            SetDefaultChartStyle(ChartStyles.OpenHighLowClose);
        }

        private void candleStickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateViewSettings(new CandleStickChart());
        }

        private void oHLCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateViewSettings(new OpenHighLowCloseChart());
        }

        private void UpdateViewSettings(ChartBase viewChart)
        {
            var elementHost = (ElementHost) tabControl1.SelectedTab.Controls[0];
            var existingChart = (ChartBase)elementHost.Child;
            var pricePeriods = existingChart.PricePeriods;
            elementHost.Child = viewChart;
            viewChart.DrawPricePeriods(pricePeriods);
            oHLCToolStripMenuItem.Checked = viewChart is OpenHighLowCloseChart;
            candleStickToolStripMenuItem.Checked = viewChart is CandleStickChart;
        }
    }
}

using System;
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
        private readonly RendererFactory _rendererFactory = new RendererFactory();
        private readonly MainFormViewModel _viewModel = new MainFormViewModel();
        private readonly DataEntryForm _dataEntryForm = new DataEntryForm();
        private List<KeyValuePair<IList<IPricePeriod>, IRenderer>> _currentChartData;
        private readonly IBrokerage _brokerage;

        public MainForm()
        {
            renderer = _rendererFactory.CreateNewRenderer();
            InitializeComponent();

            var defaultChartStyle = (ChartStyles)Enum.Parse(typeof(ChartStyles), Settings.Default.ChartStyle);

            SetDefaultChartStyle(defaultChartStyle);
            this.candleStickToolStripMenuItem.Checked = defaultChartStyle == ChartStyles.CandlestickChart;
            this.oHLCToolStripMenuItem.Checked = defaultChartStyle == ChartStyles.OpenHighLowClose;
            this.lineToolStripMenuItem.Checked = defaultChartStyle == ChartStyles.Line;
            _brokerage = new SimulatedBrokerage();
        }

        private void SetDefaultChartStyle(ChartStyles defaultChartStyle)
        {
            view_ChartStyle_CandleStick.Checked = defaultChartStyle == ChartStyles.CandlestickChart;
            view_ChartStyle_Ohlc.Checked = defaultChartStyle == ChartStyles.OpenHighLowClose;
            view_DefaultChartStyle_Line.Checked = defaultChartStyle == ChartStyles.Line;
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
            var priceSeries = new PriceSeriesFactory().ConstructPriceSeries("");
            priceSeries.AddPriceData(pricePeriods);
            _currentChartData = new List<KeyValuePair<IList<IPricePeriod>, IRenderer>>
            {
                new KeyValuePair<IList<IPricePeriod>, IRenderer>(pricePeriods, renderer)
            };
            RedrawChart(currentTab);
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
            var newChart = _rendererFactory.CreateNewRenderer();

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

        private void view_ChartStyle_Line_Click(object sender, EventArgs e)
        {
            SetDefaultChartStyle(ChartStyles.Line);
        }

        private void candleStickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateViewSettings(new CandleStickRenderer());
        }

        private void oHLCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateViewSettings(new OpenHighLowCloseRenderer());
        }

        private void lineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateViewSettings(new LineRenderer());
        }

        private void UpdateViewSettings(IRenderer renderer)
        {
            _currentChartData[0] = new KeyValuePair<IList<IPricePeriod>, IRenderer>(_currentChartData.First().Key, renderer);
            RedrawChart(tabControl1.SelectedTab);

            oHLCToolStripMenuItem.Checked = renderer is OpenHighLowCloseRenderer;
            candleStickToolStripMenuItem.Checked = renderer is CandleStickRenderer;
            lineToolStripMenuItem.Checked = renderer is LineRenderer;
        }

        private void RedrawChart(Control currentTab)
        {
            var currentElementHost = (ElementHost) currentTab.Controls[0];
            var currentChart = (Chart) currentElementHost.Child;

            currentChart.Draw(_currentChartData);
        }

        private void buyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new TradeForm(OrderType.Buy, _brokerage).ShowDialog();
            
        }

        private void sellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new TradeForm(OrderType.Sell, _brokerage).ShowDialog();
        }

        private void viewPendingOrdersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SwitchToPendingOrdersTab();
        }

        private void SwitchToPendingOrdersTab()
        {
            var pendingOrdersTab = tabControl1.TabPages.Cast<TabPage>().Single(tabPage => tabPage.Text == "Pending Orders");

            tabControl1.SelectTab(pendingOrdersTab);

            

            PopulatePendingOrders(pendingOrdersTab);
        }

        private void PopulatePendingOrders(Control pendingOrdersTab)
        {
            var openOrders = _brokerage.GetOpenOrders();

            var dataGridView = (DataGridView) pendingOrdersTab.Controls[0];
            foreach (Order openOrder in openOrders)
            {
                var rowNum = dataGridView.Rows.Add();
                var row = dataGridView.Rows[rowNum];
                row.Cells["TickerColumn"].Value = openOrder.Ticker;
                row.Cells["VolumeColumn"].Value = openOrder.Shares;
                row.Cells["PriceColumn"].Value = openOrder.Price;
                row.Cells["OrderTypeColumn"].Value = openOrder.OrderType;
                row.Cells["ExpirationColumn"].Value = openOrder.Expiration; 
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

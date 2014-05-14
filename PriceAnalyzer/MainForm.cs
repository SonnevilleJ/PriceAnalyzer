using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public partial class MainForm : Form
    {
        private readonly PriceDataManager _priceDataManager = new PriceDataManager();

        public MainForm()
        {
            InitializeComponent();
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
                DisplayInDataGrid(pricePeriods, tabName);
            }
        }

        private void DisplayInDataGrid(IList<IPricePeriod> pricePeriods, string tabName)
        {
            var tabPage = new TabPage(tabName);
            tabContainer.TabPages.Add(tabPage);
            var dataGridView = new DataGridView();
            dataGridView.Dock = DockStyle.Fill;
            tabPage.Controls.Add(dataGridView);

            dataGridView.Columns.Add("Date", "Date");
            dataGridView.Columns.Add("Open", "Open");
            dataGridView.Columns.Add("Closing price", "Closing price");
            dataGridView.Columns.Add("Low", "Low");
            dataGridView.Columns.Add("High", "High");
            dataGridView.Columns.Add("Volume", "Volume");
            
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

        private void CloseClick(object sender, EventArgs e)
        {
            Close();
        }

        private void downloadButton_Click(object sender, EventArgs e)
        {
            downloadButton.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            var ticker = TickerTextBox.Text;
            DisplayInDataGrid(_priceDataManager.DownloadPricePeriods(ticker),ticker);
            
            downloadButton.Enabled = true;
            this.Cursor = Cursors.Default;
        }
    }
}

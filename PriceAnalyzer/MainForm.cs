using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Sonneville.PriceTools.Google;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void ImportClick(object sender, System.EventArgs e)
        {
            PopulatePriceData();
        }

        private void PopulatePriceData()
        {
            var pricePeriods = OpenFileAndLoadPricePeriods();
            DisplayInDataGrid(pricePeriods);
        }

        private IList<IPricePeriod> OpenFileAndLoadPricePeriods()
        {
            var dialogResult = openFileDialog1.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                var fileName = openFileDialog1.FileName;
                var priceDataManager = new PriceDataManager();
                return priceDataManager.ParseCsvFile(fileName);
            }
            return new List<IPricePeriod>();
        }

        private void DisplayInDataGrid(IList<IPricePeriod> pricePeriods)
        {
            var tabPage = new TabPage("my first tab");
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

        private void CloseClick(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}

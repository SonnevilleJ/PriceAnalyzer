using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Sonneville.PriceTools.Data.Csv;
using Sonneville.PriceTools.Google;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public partial class MainForm : Form
    {
        private IList<IPricePeriod> _pricePeriods;

        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            OpenFileAndLoadPricePeriods();
            DisplayInDataGrid(_pricePeriods);
        }

        private void OpenFileAndLoadPricePeriods()
        {
            var dialogResult = openFileDialog1.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                _pricePeriods = new GooglePriceHistoryCsvFile(File.OpenRead(openFileDialog1.FileName)).PricePeriods;
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                MessageBox.Show("you did not open a file");
            }
        }

        private void DisplayInDataGrid(IList<IPricePeriod> pricePeriods)
        {
            dataGridView1.Columns.Add("Date", "Date");
            dataGridView1.Columns.Add("Open", "Open");
            dataGridView1.Columns.Add("Closing price", "Closing price");
            dataGridView1.Columns.Add("Low", "Low");
            dataGridView1.Columns.Add("High", "High");
            dataGridView1.Columns.Add("Volume", "Volume");
            
            for (int i = 0; i < pricePeriods.Count; i++)
            {
                var pricePeriod = pricePeriods[i];

                dataGridView1.Rows.Add();
                var row = dataGridView1.Rows[i];
                row.Cells["Date"].Value = pricePeriod.Head.Date.ToShortDateString();
                row.Cells["Closing price"].Value = pricePeriod.Close;
                row.Cells["High"].Value = pricePeriod.High;
                row.Cells["Low"].Value = pricePeriod.Low;
                row.Cells["Volume"].Value = pricePeriod.Volume;
                row.Cells["Open"].Value = pricePeriod.Open;
            }
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public class DataGridForm : GenericForm
    {
        public DataGridForm()
        {
            var tabContainer = new TabControl();
            tabContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabContainer.Location = new Point(0, -2);
            tabContainer.Name = "tabContainer";
            tabContainer.Size = new Size(828, 316);
            tabContainer.SelectedIndex = 0;
            tabContainer.TabIndex = 3;
            content = tabContainer;
            Controls.Add(tabContainer);
        }

        protected override void DisplayContent(IList<IPricePeriod> pricePeriods, string ticker)
        {
            var tabPage = new TabPage(ticker);
            ((TabControl)content).TabPages.Add(tabPage);
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
    }
}
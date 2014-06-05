using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public partial class ChartForm : GenericForm
    {
        public ChartForm()
        {
            InitializeComponent();
        }

        protected override void DisplayContent(IList<IPricePeriod> pricePeriods, string ticker)
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
            this.tabControl1.SelectedTab = newTab;

        }
    }
}

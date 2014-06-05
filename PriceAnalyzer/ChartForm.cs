using System.Collections.Generic;
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
    }
}

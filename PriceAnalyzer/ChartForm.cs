using System.Collections.Generic;

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
            highLowChart1.DrawPricePeriods(pricePeriods);
        }
    }
}

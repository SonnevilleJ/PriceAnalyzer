using System.Collections.Generic;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public partial class ChartForm : GenericForm
    {
        protected override void DisplayContent(IList<IPricePeriod> pricePeriods, string ticker)
        {
            highLowChart1.DrawPricePeriods(pricePeriods);
        }
    }
}

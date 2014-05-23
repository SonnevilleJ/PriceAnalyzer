using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Forms.Integration;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public class Chart : ElementHost
    {
        public Chart()
        {
            Child = new Canvas();
        }

        public void DrawPricePeriods(IList<IPricePeriod> pricePeriods)
        {

        }
    }
}
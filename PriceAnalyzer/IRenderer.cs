using System.Collections.Generic;
using System.Windows.Controls;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public interface IRenderer
    {
        void DrawCanvas(double highestHigh, double lowestLow, double pixelsPerDollar, double pixelsPerDay, Canvas canvas, IList<IPricePeriod> pricePeriods);
    }
}
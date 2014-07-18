using System;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public class ChartFactory
    {
        public ChartBase CreateNewChart()
        {
            var chartStyle = (ChartStyles) Enum.Parse(typeof (ChartStyles), Settings.Default.ChartStyle);

            switch (chartStyle)
            {
                case ChartStyles.CandlestickChart:
                    return new CandleStickChart();
                case ChartStyles.OpenHighLowClose:
                    return new OpenHighLowCloseChart();
            }
            throw new NotSupportedException();
        }
    }
}
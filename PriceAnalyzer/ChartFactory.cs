using System;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public class ChartFactory
    {
        public IChart CreateNewChart()
        {
            var chartStyle = GetChartStyle(Settings.Default);

            switch (chartStyle)
            {
                case ChartStyles.CandlestickChart:
                    return new CandleStickChart();
                case ChartStyles.OpenHighLowClose:
                    return new OpenHighLowCloseChart();
            }
            throw new NotSupportedException();
        }

        private static ChartStyles GetChartStyle(Settings settings)
        {
            return (ChartStyles) Enum.Parse(typeof (ChartStyles), settings.ChartStyle);
        }
    }
}
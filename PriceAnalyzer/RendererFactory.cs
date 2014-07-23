using System;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public class RendererFactory
    {
        public IRenderer CreateNewRenderer()
        {
            var chartStyle = (ChartStyles) Enum.Parse(typeof (ChartStyles), Settings.Default.ChartStyle);

            switch (chartStyle)
            {
                case ChartStyles.CandlestickChart:
                    return new CandleStickRenderer();
                case ChartStyles.OpenHighLowClose:
                    return new OpenHighLowCloseRenderer();
            }
            throw new NotSupportedException();
        }
    }
}
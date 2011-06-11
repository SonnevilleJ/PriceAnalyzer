using System;
using System.Globalization;
using System.Windows.Data;
using Sonneville.PriceTools;

namespace Sonneville.PriceChartTools
{
    public class StringToPriceSeriesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(targetType != typeof(PriceSeries)) throw new InvalidOperationException("The target must be a PriceSeries.");

            var priceSeries = new PriceSeries {Ticker = value.ToString()};
            priceSeries.DownloadPriceData(DateTime.Now.Subtract(new TimeSpan(30, 0, 0, 0)));

            return priceSeries;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

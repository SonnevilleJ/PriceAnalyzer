namespace Sonneville.PriceTools.AutomatedTrading
{
    /// <summary>
    /// Processes signals for changes in price.
    /// </summary>
    public interface ISignalProcessor
    {
        /// <summary>
        /// Signals a forthcoming change in price.
        /// </summary>
        /// <param name="priceSeries">The <see cref="IPriceSeries"/> which is expected to change.</param>
        /// <param name="direction">The anticipated direction of the price change.</param>
        /// <param name="magnitude">The anticipated percent change of the price.</param>
        void Signal(IPriceSeries priceSeries, double direction, double magnitude);
    }
}
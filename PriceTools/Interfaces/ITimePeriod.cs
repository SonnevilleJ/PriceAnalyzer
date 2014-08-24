namespace Sonneville.PriceTools
{
    public interface ITimePeriod<out TPeriodValue> : IVariableValue<TPeriodValue>
    {
        Resolution Resolution { get; }
    }
}

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   An averager creates moving averages of <see cref = "PriceSeries" /> objects.
    /// </summary>
    [Serializable]
    public class MovingAverage : Indicator
    {
        private readonly MovingAverageMethod _method;

        #region Constructors

        /// <summary>
        ///   Constructs a new Averager using the specified <see cref = "MovingAverageMethod" />
        /// </summary>
        /// <param name = "series">The <see cref="IPriceSeries"/> containing the data to be averaged.</param>
        /// <param name = "range">The number of periods to average together.</param>
        /// <param name = "movingAverageMethod">The calculation method to use when averaging.</param>
        public MovingAverage(IPriceSeries series, int range, MovingAverageMethod movingAverageMethod = MovingAverageMethod.Simple)
            : base(series, range)
        {
            _method = movingAverageMethod;
        }

        #endregion

        #region ISerializable Implementation

        /// <summary>
        ///   Deserializes a MovingAverage.
        /// </summary>
        /// <param name = "info"></param>
        /// <param name = "context"></param>
        protected MovingAverage(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if(info == null)
            {
                throw new ArgumentNullException("info");
            }

            _method = (MovingAverageMethod) info.GetValue("Method", typeof (MovingAverageMethod));
        }

        /// <summary>
        /// Gets the <see cref="MovingAverageMethod"/> used by this MovingAverage.
        /// </summary>
        public MovingAverageMethod Method
        {
            get
            {
                return _method;
            }
        }

        /// <summary>
        ///   Serializies a MovingAverage.
        /// </summary>
        /// <param name = "info"></param>
        /// <param name = "context"></param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            base.GetObjectData(info, context);
            info.AddValue("Method", _method);
        }

        #endregion

        /// <summary>
        ///   Calculates a single value of this MovingAverage.
        /// </summary>
        /// <param name = "index">The index of the value to calculate. The index of the current period is 0.</param>
        /// <returns>The value of this MovingAverage for the given period.</returns>
        protected override decimal Calculate(DateTime index)
        {
            if(!HasValue(index))
            {
                throw new ArgumentOutOfRangeException("index", index,
                                                      "Argument index must be a date within the span of this Indicator.");
            }

            switch (_method)
            {
                case MovingAverageMethod.Simple:
                    decimal sum = 0;
                    for (DateTime i = index.Subtract(new TimeSpan(Range - 1, 0, 0, 0)); i <= index; i = IncrementDate(i))
                    {
                        sum += PriceSeries[i];
                    }
                    lock (Padlock)
                    {
                        return this[index] = sum/Range;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Determines if the MovingAverage has a valid value for a given date.
        /// </summary>
        /// <param name="settlementDate">The date to check.</param>
        /// <returns>A value indicating if the MovingAverage has a valid value for the given date.</returns>
        public override bool HasValue(DateTime settlementDate)
        {
            return (settlementDate >= Head.AddDays(Range - 1) && settlementDate <= Tail);
        }
    }
}
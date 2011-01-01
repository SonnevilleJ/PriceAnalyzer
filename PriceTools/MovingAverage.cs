﻿using System;
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
        ///   Constructs a MovingAverage <see cref = "Indicator" /> for a given <see cref = "ITimeSeries" />.
        /// </summary>
        /// <param name = "series">The <see cref = "ITimeSeries" /> to average.</param>
        /// <param name = "range">The range of this MovingAverage.</param>
        public MovingAverage(ITimeSeries series, int range)
            : this(series, range, MovingAverageMethod.Simple)
        {
        }

        /// <summary>
        ///   Constructs a new Averager using the specified <see cref = "MovingAverageMethod" />
        /// </summary>
        /// <param name = "series">The IPriceSeries containing the data to be averaged.</param>
        /// <param name = "range">The number of periods to average together.</param>
        /// <param name = "movingAverageMethod">The calculation method to use when averaging.</param>
        public MovingAverage(ITimeSeries series, int range, MovingAverageMethod movingAverageMethod)
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
            _method = (MovingAverageMethod) info.GetValue("Method", typeof (MovingAverageMethod));
        }

        /// <summary>
        ///   Serializies a MovingAverage.
        /// </summary>
        /// <param name = "info"></param>
        /// <param name = "context"></param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Method", _method);
        }

        #endregion

        /// <summary>
        ///   Calculates a single value of this MovingAverage.
        /// </summary>
        /// <param name = "index">The index of the value to calculate. The index of the current period is 0.</param>
        /// <returns>The value of this MovingAverage for the given period.</returns>
        protected override decimal Calculate(int index)
        {
            switch (_method)
            {
                case MovingAverageMethod.Simple:
                    decimal sum = 0;
                    for (int i = TimeSeries.Span + (index - Range); i < TimeSeries.Span + index; i++)
                    {
                        sum += TimeSeries[i];
                    }
                    lock (Padlock)
                    {
                        return Dictionary[index] = sum/Range;
                    }
                case MovingAverageMethod.Exponential:
                    throw new NotImplementedException();
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents possible calculation methods for moving averages.
    /// </summary>
    public enum MovingAverageMethod
    {
        /// <summary>
        /// Simple Moving Average (SMA)
        /// </summary>
        Simple,
        /// <summary>
        /// Exponential Moving Average (EMA)
        /// </summary>
        Exponential
    }
}
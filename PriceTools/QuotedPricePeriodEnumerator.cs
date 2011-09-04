﻿using System;
using System.Collections;
using System.Linq;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Enumerator for a <see cref="QuotedPricePeriod"/>.
    /// </summary>
    public class QuotedPricePeriodEnumerator : IEnumerator
    {
        private readonly QuotedPricePeriod _quotedPricePeriod;
        private int _index;

        #region Constructor

        internal QuotedPricePeriodEnumerator(QuotedPricePeriod quotedPricePeriod)
        {
            _quotedPricePeriod = quotedPricePeriod;
            Reset();
        }

        #endregion

        #region Implementation of IEnumerator

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>
        /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception><filterpriority>2</filterpriority>
        public bool MoveNext()
        {
            return ++_index < _quotedPricePeriod.PriceQuotes.Count;
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception><filterpriority>2</filterpriority>
        public void Reset()
        {
            _index = -1;
        }

        /// <summary>
        /// Gets the current element in the collection.
        /// </summary>
        /// <returns>
        /// The current element in the collection.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element.</exception><filterpriority>2</filterpriority>
        public object Current
        {
            get { return _quotedPricePeriod.PriceQuotes.ElementAt(_index).Price; }
        }

        #endregion
    }
}
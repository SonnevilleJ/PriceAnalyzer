//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.Serialization;

//namespace Sonneville.PriceTools.Internal
//{
//    [Serializable]
//    internal class DualIndexedCollection : ICollection<decimal>, ISerializable
//    {
//        #region Private Members
        
//        private readonly IDictionary<DateTime, decimal> _collection;
//        private readonly DateTime _zero;

//        #endregion

//        #region Constructors

//        public DualIndexedCollection(ITimeSeries timeSeries)
//        {
//            _collection = new Dictionary<DateTime, decimal>(timeSeries.Periods);
//            _zero = timeSeries.Tail;
//            for (int i = timeSeries.Periods - 1; i >= 0; i--)
//            {
//                DateTime asOfDate = ConvertIndexToDate(i);
//                _collection[asOfDate] = timeSeries[asOfDate];
//            }
//        }

//        #endregion

//        #region Private Methods

//        internal DateTime ConvertIndexToDate(int index)
//        {
//            return _zero.AddDays(0.0 - index);
//        }

//        #endregion

//        #region Accessors

//        public decimal this[int index]
//        {
//            get { return this[ConvertIndexToDate(index)]; }
//            set { this[ConvertIndexToDate(index)] = value; }
//        }

//        public decimal this[DateTime index]
//        {
//            get { return _collection[index]; }
//            set { _collection[index] = value; }
//        }

//        #endregion

//        #region Implementation of IEnumerable

//        /// <summary>
//        ///   Returns an enumerator that iterates through the collection.
//        /// </summary>
//        /// <returns>
//        ///   A <see cref = "T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
//        /// </returns>
//        /// <filterpriority>1</filterpriority>
//        public IEnumerator<decimal > GetEnumerator()
//        {
//            return (IEnumerator<decimal>)_collection.GetEnumerator();
//        }

//        /// <summary>
//        ///   Returns an enumerator that iterates through a collection.
//        /// </summary>
//        /// <returns>
//        ///   An <see cref = "T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
//        /// </returns>
//        /// <filterpriority>2</filterpriority>
//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return GetEnumerator();
//        }

//        #endregion

//        #region Implementation of ICollection<T>

//        /// <summary>
//        ///   Adds an item to the <see cref = "ICollection" />.
//        /// </summary>
//        /// <param name = "item">The object to add to the <see cref = "ICollection" />.</param>
//        /// <exception cref = "T:System.NotSupportedException">The <see cref = "ICollection" /> is read-only.</exception>
//        public void Add(decimal item)
//        {
//            throw new NotImplementedException();
//        }

//        /// <summary>
//        ///   Removes all items from the <see cref = "ICollection" />.
//        /// </summary>
//        /// <exception cref = "T:System.NotSupportedException">The <see cref = "ICollection" /> is read-only. </exception>
//        public void Clear()
//        {
//            throw new NotImplementedException();
//        }

//        /// <summary>
//        ///   Determines whether the <see cref = "ICollection" /> contains a specific value.
//        /// </summary>
//        /// <returns>
//        ///   true if <paramref name = "item" /> is found in the <see cref = "ICollection" />; otherwise, false.
//        /// </returns>
//        /// <param name = "item">The object to locate in the <see cref = "ICollection" />.</param>
//        public bool Contains(decimal item)
//        {
//            return _collection.Cast<decimal>().Contains(item);
//        }

//        /// <summary>
//        ///   Copies the elements of the <see cref = "ICollection" /> to an <see cref = "T:System.Array" />, starting at a particular <see cref = "T:System.Array" /> index.
//        /// </summary>
//        /// <param name = "array">The one-dimensional <see cref = "T:System.Array" /> that is the destination of the elements copied from <see cref = "ICollection" />. The <see cref = "T:System.Array" /> must have zero-based indexing.</param>
//        /// <param name = "arrayIndex">The zero-based index in <paramref name = "array" /> at which copying begins.</param>
//        /// <exception cref = "T:System.ArgumentNullException"><paramref name = "array" /> is null.</exception>
//        /// <exception cref = "T:System.ArgumentOutOfRangeException"><paramref name = "arrayIndex" /> is less than 0.</exception>
//        /// <exception cref = "T:System.ArgumentException"><paramref name = "array" /> is multidimensional.-or-The number of elements in the source <see cref = "ICollection" /> is greater than the available space from <paramref name = "arrayIndex" /> to the end of the destination <paramref name = "array" />.-or-Type <paramref name = "array" /> cannot be cast automatically to the type of the destination <paramref name = "array" />.</exception>
//        public void CopyTo(decimal [] array, int arrayIndex)
//        {
//            KeyValuePair<DateTime, decimal >[] kvp = new KeyValuePair<DateTime, decimal>[_collection.Count];
//            _collection.CopyTo(kvp, arrayIndex);
//            kvp.CopyTo(array, arrayIndex);
//        }

//        /// <summary>
//        ///   Removes the first occurrence of a specific object from the <see cref = "ICollection" />.
//        /// </summary>
//        /// <returns>
//        ///   true if <paramref name = "item" /> was successfully removed from the <see cref = "ICollection" />; otherwise, false. This method also returns false if <paramref name = "item" /> is not found in the original <see cref = "ICollection" />.
//        /// </returns>
//        /// <param name = "item">The object to remove from the <see cref = "ICollection" />.</param>
//        /// <exception cref = "T:System.NotSupportedException">The <see cref = "ICollection" /> is read-only.</exception>
//        public bool Remove(decimal item)
//        {
//            throw new NotImplementedException();
//        }

//        /// <summary>
//        ///   Gets the number of elements contained in the <see cref = "ICollection" />.
//        /// </summary>
//        /// <returns>
//        ///   The number of elements contained in the <see cref = "ICollection" />.
//        /// </returns>
//        public int Count
//        {
//            get { return _collection.Count; }
//        }

//        /// <summary>
//        ///   Gets a value indicating whether the <see cref = "ICollection" /> is read-only.
//        /// </summary>
//        /// <returns>
//        ///   true if the <see cref = "ICollection" /> is read-only; otherwise, false.
//        /// </returns>
//        public bool IsReadOnly
//        {
//            get { return false; }
//        }

//        #endregion

//        #region Implementation of ISerializable

//        protected DualIndexedCollection(SerializationInfo info, StreamingContext context)
//        {
//            _collection = (IDictionary<DateTime, decimal>) info.GetValue("Collection", typeof (IDictionary<DateTime, decimal>));
//        }

//        /// <summary>
//        ///   Populates a <see cref = "T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the target object.
//        /// </summary>
//        /// <param name = "info">The <see cref = "T:System.Runtime.Serialization.SerializationInfo" /> to populate with data. </param>
//        /// <param name = "context">The destination (see <see cref = "T:System.Runtime.Serialization.StreamingContext" />) for this serialization. </param>
//        /// <exception cref = "T:System.Security.SecurityException">The caller does not have the required permission. </exception>
//        public void GetObjectData(SerializationInfo info, StreamingContext context)
//        {
//            info.AddValue("Collection", _collection, typeof (ICollection));
//        }

//        #endregion
//    }
//}

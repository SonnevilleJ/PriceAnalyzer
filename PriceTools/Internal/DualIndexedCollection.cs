using System;
using System.Collections;
using System.Collections.Generic;

namespace Sonneville.PriceTools.Internal
{
    internal class DualIndexedCollection<T> : SortedSet<T>
    {
        #region Private Members

        private ICollection _collection = new Dictionary<DateTime, T>();
        private ICollection _map = new Dictionary<DateTime, int>();

        #endregion

        #region Constructors

        public DualIndexedCollection()
        {
        }

        public DualIndexedCollection(int capacity)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Accessors

        public T this[int index]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public T this[DateTime index]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #endregion
    }
}

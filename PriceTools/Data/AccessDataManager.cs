using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace Sonneville.PriceTools.Data
{
    /// <summary>
    /// An IDataManager to interact with an Access 2007 database.
    /// </summary>
    public class AccessDataManager : IDataManager
    {
        private OleDbConnection _conn;

        /// <summary>
        /// Constructs an IDataManager to interact with an Access 2007 database.
        /// </summary>
        /// <param name="conn">The <see cref="OleDbConnection"/> to the database.</param>
        public AccessDataManager(OleDbConnection conn)
        {
            _conn = conn;
        }

        public void StoreTransactions(List<ITransaction> transactions)
        {
            OleDbCommandBuilder builder = new OleDbCommandBuilder();


        }

        public decimal GetPrice(string ticker, DateTime date)
        {
            throw new NotImplementedException();
        }

        public List<ITransaction> GetTransactions(DateTime head, DateTime tail)
        {
            return GetTransactions("*", head, tail);
        }

        public List<ITransaction> GetTransactions(string ticker, DateTime head, DateTime tail)
        {
            throw new NotImplementedException();
        }

        public List<ITransaction> GetTransactions(OrderType type, DateTime head, DateTime tail)
        {
            return GetTransactions(type, "*", head, tail);
        }

        public List<ITransaction> GetTransactions(OrderType type, string ticker, DateTime head, DateTime tail)
        {
            throw new NotImplementedException();
        }
    }
}

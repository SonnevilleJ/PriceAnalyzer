using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace Sonneville.PriceTools
{
    internal class AccessDataReader
    {
        private const string ConnString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\dev\PriceAnalyzer\database.accdb;Persist Security Info=False;";

        public DataSet GetPrice(string ticker, DateTime head, DateTime tail)
        {
            OleDbConnection conn = new OleDbConnection(ConnString);
            string sql = string.Format(
                "SELECT D, O, H, L, C, Volume FROM {0} WHERE D BETWEEN #{1}# and #{2}#",
                ticker,
                head.ToShortDateString(),
                tail.ToShortDateString());
            DataSet ds = new DataSet();
            OleDbDataAdapter adapter;
            QueryDatabase(conn, sql, out adapter);
            adapter.Fill(ds);
            return ds;
        }

        private void QueryDatabase(OleDbConnection connection, string query, out OleDbDataAdapter da)
        {
            if(!string.IsNullOrEmpty(query) && !query.StartsWith("SELECT", StringComparison.CurrentCultureIgnoreCase))
            {
                throw new ArgumentException("Query is not a SELECT query.", "query");
            }
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                da = new OleDbDataAdapter(query, connection);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }
    }
}

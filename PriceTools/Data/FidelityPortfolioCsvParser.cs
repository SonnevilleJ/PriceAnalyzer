using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using LumenWorks.Framework.IO.Csv;

namespace Sonneville.PriceTools.Data
{
    /// <summary>
    /// Parses an <see cref="IPortfolio"/> from Fidelity CSV data.
    /// </summary>
    public class FidelityPortfolioCsvParser : IPortfolioCsvParser
    {
        #region Private Members

        private static readonly DataColumn OpenColumn = new DataColumn("Open", typeof(decimal));
        private static readonly DataColumn HighColumn = new DataColumn("Open", typeof(decimal));
        private static readonly DataColumn LowColumn = new DataColumn("Open", typeof(decimal));
        private static readonly DataColumn CloseColumn = new DataColumn("Open", typeof(decimal));
        private static readonly DataColumn VolumeColumn = new DataColumn("Open", typeof(long));

        #endregion

        /// <summary>
        /// Parses an <see cref="IPortfolio"/> from a given Fidelity CSV data stream.
        /// </summary>
        /// <param name="csvStream">The <see cref="Stream"/> to parse.</param>
        /// <returns>An <see cref="IPortfolio"/>.</returns>
        public IPortfolio ParsePortfolio(Stream csvStream)
        {
            CsvReader reader = new CsvReader(new StreamReader(csvStream), true);
            string[] headers = reader.GetFieldHeaders();

            DataTable table = ParsePortfolioToDataTable(reader);
            return null;
        }

        /// <summary>
        /// Parses a <see cref="DataTable"/> from a given Fidelity CSV data stream.
        /// </summary>
        /// <param name="reader">The <see cref="Stream"/> to parse.</param>
        /// <returns>A <see cref="DataTable"/> of the CSV data.</returns>
        public DataTable ParsePortfolioToDataTable(CsvReader reader)
        {
            DataTable table = InitializePortfolioTable();
            string[] headers = GetHeaders(reader);

            while (reader.ReadNextRecord())
            {
                DataRow row = table.NewRow();
                row.BeginEdit();

                row.EndEdit();
            }

            return table;
        }

        private string[] GetHeaders(CsvReader reader)
        {
            throw new NotImplementedException();
        }

        private DataTable InitializePortfolioTable()
        {
            throw new NotImplementedException();
        }
    }
}

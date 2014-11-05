using System.IO;
using LumenWorks.Framework.IO.Csv;

namespace Sonneville.PriceTools.Data
{
    public class CsvReaderWrapper : ICsvReader
    {
        private readonly CsvReader _reader;

        public CsvReaderWrapper(Stream reader)
        {
            _reader = new CsvReader(new StreamReader(reader), true);
        }

        public bool ReadNextRecord()
        {
            return _reader.ReadNextRecord();
        }

        public string this[int i]
        {
            get { return _reader[i]; }
        }

        public string[] GetFieldHeaders()
        {
            return _reader.GetFieldHeaders();
        }

        public void Dispose()
        {
            _reader.Dispose();
        }
    }
}

using System;

namespace Sonneville.PriceTools.Data
{
    public interface ICsvReader : IDisposable
    {
        bool ReadNextRecord();
        string this[int i] { get; }
        string[] GetFieldHeaders();
    }
}
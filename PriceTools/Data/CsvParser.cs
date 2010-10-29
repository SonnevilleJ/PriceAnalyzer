using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Sonneville.PriceTools.Data
{
    /// <summary>
    /// Use the CsvParser class to parse a CSV file containing orders for financial securities.
    /// </summary>
    public static class CsvParser
    {
        /// <summary>
        /// Parses a CSV file containing orders for financial securities.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to the CSV file.</param>
        /// <param name="leaveOpen">Should the CsvParser leave the <see cref="Stream"/> open? Default = false.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="ITransaction"/> objects.</returns>
        public static List<ITransaction> ParseCsv(Stream stream, bool leaveOpen = false)
        {
            List<ITransaction> data = new List<ITransaction>();
            List<ITransaction> transactions = new List<ITransaction>();
            StreamReader reader = new StreamReader(stream);
            string line = null;
            bool doHeaders = true;
            
            try
            {
// ReSharper disable ConditionIsAlwaysTrueOrFalse
                while ((line == reader.ReadLine()) != null)
// ReSharper restore ConditionIsAlwaysTrueOrFalse
                {
// ReSharper disable PossibleNullReferenceException
                    string[] elements = line.Split(',');
// ReSharper restore PossibleNullReferenceException
                    Hashtable key = new Hashtable(6);
                    Hashtable table = new Hashtable(6);
                    if (doHeaders)
                    {
                        for (int i = 0; i < elements.Length; i++)
                        {
                            switch (elements[i].ToLower())
                            {
                                case "trade date":
                                    key.Add("date", i);
                                    break;
                                case "transaction type":
                                    key.Add("type", i);
                                    break;
                                case "symbol":
                                    key.Add("symbol", i);
                                    break;
                                case "quantity":
                                    key.Add("shares", i);
                                    break;
                                case "price ($)":
                                    key.Add("price", i);
                                    break;
                                case "commission ($)":
                                    key.Add("commission", i);
                                    break;
                                default:
                                    // ignore other columns
                                    break;
                            }
                        }
                        doHeaders = false;
                    }
                    else
                    {
                        for (int i = 0; i < elements.Length; i++)
                        {
                            table.Add(key[i], elements[i]);
                        }
                    }
                    switch ((string)table["type"])
                    {
                        case "Reinvestment":
                            table["type"] = "Buy";
                            continue;
                        case "Buy":
                        case "Sell":
                            transactions.Add(
                                new Transaction((DateTime) table["date"],
                                                (OrderType) table["type"],
                                                (string) table["symbol"],
                                                (decimal) table["price"],
                                                (double) table["shares"],
                                                (decimal) table["commission"]));
                            break;
                        default:
                            break;
                    }
                    return transactions;
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                if (!leaveOpen)
                {
                    reader.Close();
                }
            }
            return data;
        }
    }
}

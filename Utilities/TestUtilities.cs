using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects.DataClasses;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;

namespace Sonneville.Utilities
{
    public static class TestUtilities
    {
        /// <summary>
        ///   Performs a binary serialization and deserialization of an object to a <see cref = "Stream" />.
        /// </summary>
        /// <param name = "obj">The object to serialize.</param>
        public static object Serialize(object obj)
        {
            using (Stream stream = GetTemporaryFile())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Flush();
                stream.Position = 0; // reset cursor to enable immediate deserialization

                return formatter.Deserialize(stream);
            }
        }

        private static FileStream GetTemporaryFile()
        {
            string path = Path.GetTempPath();
            string filename = Guid.NewGuid() + ".tmp";
            return File.Open(string.Concat(path, filename), FileMode.CreateNew);
        }

        private static void VerifyEntitySerialize(EntityObject target, string entitySetName)
        {
            using (var container = new Container())
            {
                Assert.AreEqual(EntityState.Detached, target.EntityState);

                container.AddObject(entitySetName, target);
                Assert.AreEqual(EntityState.Added, target.EntityState);

                container.SaveChanges();
                Assert.AreEqual(EntityState.Unchanged, target.EntityState);

                container.DeleteObject(target);
                Assert.AreEqual(EntityState.Deleted, target.EntityState);

                container.SaveChanges();
                Assert.AreEqual(EntityState.Detached, target.EntityState);
            }
        }

        public static void VerifyTransactionEntity(ITransaction transaction)
        {
            VerifyEntitySerialize((Transaction)transaction, "Transactions");
        }

        public static void VerifyCashAccountEntity(ICashAccount cashAccount)
        {
            VerifyEntitySerialize((CashAccount) cashAccount, "CashAccounts");
        }

        public static void VerifyPositionEntity(IPosition position)
        {
            VerifyEntitySerialize((Position) position, "Positions");
        }

        public static void VerifyPortfolioEntity(IPortfolio portfolio)
        {
            VerifyEntitySerialize((Portfolio) portfolio, "Portfolios");
        }

        public static void VerifyPriceQuoteEntity(IPriceQuote priceQuote)
        {
            VerifyEntitySerialize((PriceQuote) priceQuote, "PriceQuotes");
        }

        public static void VerifyPricePeriodEntity(IPricePeriod pricePeriod)
        {
            VerifyEntitySerialize((PricePeriod) pricePeriod, "PricePeriods");
        }

        public static void VerifyPriceSeriesEntity(IPriceSeries priceSeries)
        {
            IList<PricePeriod> list = priceSeries.PricePeriods.ToList();

            VerifyEntitySerialize((PriceSeries) priceSeries, "PricePeriods");

            using (var db = new Container())
            {
                foreach (PricePeriod pricePeriod in db.PricePeriods)
                {
                    if (list.Contains(pricePeriod))
                    {
                        db.PricePeriods.DeleteObject(pricePeriod);
                    }
                }
                db.SaveChanges();
            }
        }
    }
}

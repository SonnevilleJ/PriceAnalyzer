using System;
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
        public static T Serialize<T>(T obj) where T : class
        {
            using (Stream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Flush();
                stream.Position = 0; // reset cursor to enable immediate deserialization

                return formatter.Deserialize(stream) as T;
            }
        }

        #region Entity Framework

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
            EntityCollection<PricePeriod> list = ((PriceSeries)priceSeries).DataPeriods;
            try
            {
                VerifyEntitySerialize((PriceSeries)priceSeries, "PricePeriods");
            }
            finally
            {
                using (var db = new Container())
                {
                    foreach (PricePeriod pricePeriod in db.PricePeriods.Where(pricePeriod => list.Contains(pricePeriod)))
                    {
                        db.PricePeriods.DeleteObject(pricePeriod);
                    }
                    db.SaveChanges();
                }
            }
        }

        #endregion

        #region Price Period tools

        public static PricePeriod CreatePeriod1()
        {
            DateTime head = new DateTime(2011, 3, 11);
            DateTime tail = head.AddDays(1);
            const decimal open = 100.00m;
            const decimal high = 110.00m;
            const decimal low = 90.00m;
            const decimal close = 100.00m;
            const long volume = 20000;

            return PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);
        }

        public static PricePeriod CreatePeriod2()
        {
            DateTime head = new DateTime(2011, 3, 12);
            DateTime tail = head.AddDays(1);
            const decimal open = 100.00m;
            const decimal high = 120.00m;
            const decimal low = 100.00m;
            const decimal close = 110.00m;

            return PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close);
        }

        public static PricePeriod CreatePeriod3()
        {
            DateTime head = new DateTime(2011, 3, 13);
            DateTime tail = head.AddDays(1);
            const decimal open = 110.00m;
            const decimal high = 110.00m;
            const decimal low = 80.00m;
            const decimal close = 90.00m;
            const long volume = 10000;

            return PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);
        }

        #endregion

        #region Price Quote tools

        public static PriceQuote CreateQuote1()
        {
            return new PriceQuote
            {
                SettlementDate = DateTime.Parse("2/28/2011 9:30 AM"),
                Price = 10,
                Volume = 50
            };
        }

        public static PriceQuote CreateQuote2()
        {
            return new PriceQuote
            {
                SettlementDate = DateTime.Parse("3/1/2011 10:00 AM"),
                Price = 9,
                Volume = 60
            };
        }

        public static PriceQuote CreateQuote3()
        {
            return new PriceQuote
            {
                SettlementDate = DateTime.Parse("3/2/2011 2:00 PM"),
                Price = 14,
                Volume = 50
            };
        }

        public static PriceQuote CreateQuote4()
        {
            return new PriceQuote
            {
                SettlementDate = DateTime.Parse("3/2/2011 4:00 PM"),
                Price = 11,
                Volume = 30
            };
        }

        #endregion
    }
}

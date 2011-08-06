using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Sonneville.PriceTools.Extensions;

namespace Sonneville.PriceToolsTest
{
    /// <summary>
    ///This is a test class for DateTimeExtensionsTest and is intended
    ///to contain all DateTimeExtensionsTest Unit Tests
    ///</summary>
    [TestClass]
    public class DateTimeExtensionsTest
    {
        /// <summary>
        /// A test for GetFollowingOpen
        /// </summary>
        [TestMethod]
        public void GetFollowingOpenTestFromMidnight()
        {
            var dateTime = new DateTime(2011, 8, 2);
            var expected = new DateTime(2011, 8, 3);
            
            var actual = DateTimeExtensions.GetFollowingOpen(dateTime);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetFollowingOpen
        /// </summary>
        [TestMethod]
        public void GetFollowingOpenTestFromEndOfDay()
        {
            var dateTime = new DateTime(2011, 8, 2, 23, 59, 59);
            var expected = new DateTime(2011, 8, 3);
            
            var actual = DateTimeExtensions.GetFollowingOpen(dateTime);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetFollowingOpen
        /// </summary>
        [TestMethod]
        public void GetFollowingOpenTestFromSaturday()
        {
            var dateTime = new DateTime(2011, 8, 6);
            var expected = new DateTime(2011, 8, 8);

            var actual = DateTimeExtensions.GetFollowingOpen(dateTime);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetFollowingClose
        /// </summary>
        [TestMethod]
        public void GetFollowingCloseTestFromNoon()
        {
            var dateTime = new DateTime(2011, 8, 2, 12, 0, 0);
            var expected = new DateTime(2011, 8, 2, 23, 59, 59);
            
            var actual = DateTimeExtensions.GetFollowingClose(dateTime);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetFollowingClose
        /// </summary>
        [TestMethod]
        public void GetFollowingCloseTestFromEndOfDay()
        {
            var dateTime = new DateTime(2011, 8, 2, 23, 59, 59);
            var expected = new DateTime(2011, 8, 3, 23, 59, 59);
            
            var actual = DateTimeExtensions.GetFollowingClose(dateTime);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetFollowingClose
        /// </summary>
        [TestMethod]
        public void GetFollowingCloseTestFromSaturday()
        {
            var dateTime = new DateTime(2011, 8, 6);
            var expected = new DateTime(2011, 8, 8, 23, 59, 59);
            
            var actual = DateTimeExtensions.GetFollowingClose(dateTime);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetFollowingWeekClose
        /// </summary>
        [TestMethod]
        public void GetFollowingWeekCloseTestFromWednesday()
        {
            var dateTime = new DateTime(2011, 8, 2);
            var expected = new DateTime(2011, 8, 5, 23, 59, 59);
            
            var actual = DateTimeExtensions.GetFollowingWeekClose(dateTime);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetFollowingWeekClose
        /// </summary>
        [TestMethod]
        public void GetFollowingWeekCloseTestFromFridayOpen()
        {
            var dateTime = new DateTime(2011, 8, 5);
            var expected = new DateTime(2011, 8, 5, 23, 59, 59);
            
            var actual = DateTimeExtensions.GetFollowingWeekClose(dateTime);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetFollowingWeekClose
        /// </summary>
        [TestMethod]
        public void GetFollowingWeekCloseTestFromSaturday()
        {
            var dateTime = new DateTime(2011, 8, 6);
            var expected = new DateTime(2011, 8, 12, 23, 59, 59);
            
            var actual = DateTimeExtensions.GetFollowingWeekClose(dateTime);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetFollowingWeeklyOpen
        /// </summary>
        [TestMethod]
        public void GetFollowingWeekOpenTestFromWednesday()
        {
            var dateTime = new DateTime(2011, 8, 2);
            var expected = new DateTime(2011, 8, 8);
            
            var actual = DateTimeExtensions.GetFollowingWeeklyOpen(dateTime);
            
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetFollowingWeeklyOpen
        /// </summary>
        [TestMethod]
        public void GetFollowingWeekOpenTestFromSaturday()
        {
            var dateTime = new DateTime(2011, 8, 6);
            var expected = new DateTime(2011, 8, 8);

            var actual = DateTimeExtensions.GetFollowingWeeklyOpen(dateTime);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetFollowingWeeklyOpen
        /// </summary>
        [TestMethod]
        public void GetFollowingWeekOpenTestFromMondayOpen()
        {
            var dateTime = new DateTime(2011, 8, 8);
            var expected = new DateTime(2011, 8, 15);
            
            var actual = DateTimeExtensions.GetFollowingWeeklyOpen(dateTime);
            
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetCurrentOrFollowingTradingDayTestMonday()
        {
            var dateTime = new DateTime(2011, 8, 1);
            var expected = dateTime;

            var actual = DateTimeExtensions.GetCurrentOrFollowingTradingDay(dateTime);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetCurrentOrFollowingTradingDayTestTuesday()
        {
            var dateTime = new DateTime(2011, 8, 2);
            var expected = dateTime;

            var actual = DateTimeExtensions.GetCurrentOrFollowingTradingDay(dateTime);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetCurrentOrFollowingTradingDayTestWednesday()
        {
            var dateTime = new DateTime(2011, 8, 3);
            var expected = dateTime;

            var actual = DateTimeExtensions.GetCurrentOrFollowingTradingDay(dateTime);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetCurrentOrFollowingTradingDayTestThursday()
        {
            var dateTime = new DateTime(2011, 8, 4);
            var expected = dateTime;

            var actual = DateTimeExtensions.GetCurrentOrFollowingTradingDay(dateTime);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetCurrentOrFollowingTradingDayTestFriday()
        {
            var dateTime = new DateTime(2011, 8, 5);
            var expected = dateTime;

            var actual = DateTimeExtensions.GetCurrentOrFollowingTradingDay(dateTime);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetCurrentOrFollowingTradingDayTestSaturday()
        {
            var dateTime = new DateTime(2011, 8, 6);
            var expected = new DateTime(2011, 8, 8);

            var actual = DateTimeExtensions.GetCurrentOrFollowingTradingDay(dateTime);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetCurrentOrFollowingTradingDayTestSunday()
        {
            var dateTime = new DateTime(2011, 8, 7);
            var expected = new DateTime(2011, 8, 8);

            var actual = DateTimeExtensions.GetCurrentOrFollowingTradingDay(dateTime);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetBeginningOfTradingDay
        ///</summary>
        [TestMethod]
        public void GetBeginningOfTradingDayTest()
        {
            var date = new DateTime(2011, 8, 5);
            var expected = date.Date;
            
            var actual = DateTimeExtensions.GetMostRecentOpen(date);
            
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetBeginningOfTradingDay
        ///</summary>
        [TestMethod]
        public void GetBeginningOfTradingDayFromNoonTest()
        {
            var date = new DateTime(2011, 8, 5, 12, 0, 0);
            var expected = date.Date;

            var actual = DateTimeExtensions.GetMostRecentOpen(date);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetFollowingClose
        ///</summary>
        [TestMethod]
        public void GetFollowingCloseFromCloseTest()
        {
            var date = new DateTime(2011, 8, 5, 23, 59, 59);
            var expected = new DateTime(2011, 8, 8, 23, 59, 59);

            var actual = DateTimeExtensions.GetFollowingClose(date);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetFollowingClose
        ///</summary>
        [TestMethod]
        public void GetFollowingCloseFromNoonTest()
        {
            var date = new DateTime(2011, 8, 5, 12, 0, 0);
            var expected = new DateTime(2011, 8, 5, 23, 59, 59);

            var actual = DateTimeExtensions.GetFollowingClose(date);
            
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetFollowingMonthOpenTest()
        {
            var date = new DateTime(2011, 8, 5);
            var expected = new DateTime(2011, 9, 1);

            var actual = DateTimeExtensions.GetFollowingMonthlyOpen(date);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetFollowingMonthCloseTest()
        {
            var date = new DateTime(2011, 8, 5);
            var expected = new DateTime(2011, 9, 30).GetFollowingClose();

            var actual = DateTimeExtensions.GetFollowingMonthlyClose(date);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetMostRecentMonthOpenTest()
        {
            var date = new DateTime(2011, 8, 5);
            var expected = new DateTime(2011, 8, 1);

            var actual = DateTimeExtensions.GetMostRecentMonthlyOpen(date);

            Assert.AreEqual(expected, actual);
        }
    }
}

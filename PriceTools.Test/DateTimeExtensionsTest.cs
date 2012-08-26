using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Extensions;

namespace Test.Sonneville.PriceTools
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
            var target = new DateTime(2011, 8, 2);

            var expected = new DateTime(2011, 8, 3);
            var actual = target.GetFollowingOpen();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetFollowingOpen
        /// </summary>
        [TestMethod]
        public void GetFollowingOpenTestFromEndOfDay()
        {
            var target = new DateTime(2011, 8, 2, 23, 59, 59);
            
            var expected = new DateTime(2011, 8, 3);
            var actual = target.GetFollowingOpen();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetFollowingOpen
        /// </summary>
        [TestMethod]
        public void GetFollowingOpenTestFromSaturday()
        {
            var target = new DateTime(2011, 8, 6);
            
            var expected = new DateTime(2011, 8, 8);
            var actual = target.GetFollowingOpen();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetFollowingClose
        /// </summary>
        [TestMethod]
        public void GetFollowingCloseTestFromNoon()
        {
            var target = new DateTime(2011, 8, 2, 12, 0, 0);
            
            var expected = new DateTime(2011, 8, 2, 23, 59, 59);
            var actual = target.GetFollowingClose();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetFollowingClose
        /// </summary>
        [TestMethod]
        public void GetFollowingCloseTestFromEndOfDay()
        {
            var target = new DateTime(2011, 8, 2, 23, 59, 59);
            
            var expected = new DateTime(2011, 8, 3, 23, 59, 59);
            var actual = target.GetFollowingClose();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetFollowingClose
        /// </summary>
        [TestMethod]
        public void GetFollowingCloseTestFromSaturday()
        {
            var target = new DateTime(2011, 8, 6);
            
            var expected = new DateTime(2011, 8, 8, 23, 59, 59);
            var actual = target.GetFollowingClose();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetFollowingWeeklyClose
        /// </summary>
        [TestMethod]
        public void GetFollowingWeekCloseTestFromWednesday()
        {
            var target = new DateTime(2011, 8, 2);
            
            var expected = new DateTime(2011, 8, 5, 23, 59, 59);
            var actual = target.GetFollowingWeeklyClose();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetFollowingWeeklyClose
        /// </summary>
        [TestMethod]
        public void GetFollowingWeekCloseTestFromFridayOpen()
        {
            var target = new DateTime(2011, 8, 5);
            
            var expected = new DateTime(2011, 8, 5, 23, 59, 59);
            var actual = target.GetFollowingWeeklyClose();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetFollowingWeeklyClose
        /// </summary>
        [TestMethod]
        public void GetFollowingWeekCloseTestFromSaturday()
        {
            var target = new DateTime(2011, 8, 6);
            
            var expected = new DateTime(2011, 8, 12, 23, 59, 59);
            var actual = target.GetFollowingWeeklyClose();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetFollowingWeeklyOpen
        /// </summary>
        [TestMethod]
        public void GetFollowingWeekOpenTestFromWednesday()
        {
            var target = new DateTime(2011, 8, 2);
            
            var expected = new DateTime(2011, 8, 8);
            var actual = target.GetFollowingWeeklyOpen();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetFollowingWeeklyOpen
        /// </summary>
        [TestMethod]
        public void GetFollowingWeekOpenTestFromSaturday()
        {
            var target = new DateTime(2011, 8, 6);
            
            var expected = new DateTime(2011, 8, 8);
            var actual = target.GetFollowingWeeklyOpen();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetFollowingWeeklyOpen
        /// </summary>
        [TestMethod]
        public void GetFollowingWeekOpenTestFromMondayOpen()
        {
            var target = new DateTime(2011, 8, 8);
            
            var expected = new DateTime(2011, 8, 15);
            var actual = target.GetFollowingWeeklyOpen();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetMostRecentOpen
        ///</summary>
        [TestMethod]
        public void GetMostRecentOpenFridayTest()
        {
            var target = new DateTime(2011, 8, 5);
            
            var expected = target.Date;
            var actual = target.GetMostRecentOpen();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetMostRecentOpen
        ///</summary>
        [TestMethod]
        public void GetMostRecentOpenSaturdayTest()
        {
            var target = new DateTime(2011, 8, 6);

            var expected = new DateTime(2011, 8, 5);
            var actual = target.GetMostRecentOpen();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetMostRecentOpen
        ///</summary>
        [TestMethod]
        public void GetMostRecentOpenFromNoonTest()
        {
            var target = new DateTime(2011, 8, 5, 12, 0, 0);
            
            var expected = target.Date;
            var actual = target.GetMostRecentOpen();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetMostRecentClose
        ///</summary>
        [TestMethod]
        public void GetMostRecentCloseFridayTest()
        {
            var target = new DateTime(2011, 8, 5);  // Friday

            var expected = new DateTime(2011, 8, 4).GetFollowingClose();
            var actual = target.GetMostRecentClose();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetMostRecentClose
        ///</summary>
        [TestMethod]
        public void GetMostRecentCloseSaturdayTest()
        {
            var target = new DateTime(2011, 7, 2, 23, 59, 59);

            var expected = new DateTime(2011, 7, 1).GetFollowingClose();
            var actual = target.GetMostRecentClose();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetMostRecentClose
        ///</summary>
        [TestMethod]
        public void GetMostRecentCloseSundayTest()
        {
            var target = new DateTime(2011, 8, 7);

            var expected = new DateTime(2011, 8, 5).GetFollowingClose();
            var actual = target.GetMostRecentClose();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetMostRecentClose
        ///</summary>
        [TestMethod]
        public void GetMostRecentCloseFromNoonTest()
        {
            var target = new DateTime(2011, 8, 5, 12, 0, 0);

            var expected = new DateTime(2011, 8, 4).GetFollowingClose();
            var actual = target.GetMostRecentClose();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetFollowingClose
        ///</summary>
        [TestMethod]
        public void GetFollowingCloseFromCloseTest()
        {
            var target = new DateTime(2011, 8, 5, 23, 59, 59);
            
            var expected = new DateTime(2011, 8, 8, 23, 59, 59);
            var actual = target.GetFollowingClose();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetFollowingClose
        ///</summary>
        [TestMethod]
        public void GetFollowingCloseFromNoonTest()
        {
            var target = new DateTime(2011, 8, 5, 12, 0, 0);
            
            var expected = new DateTime(2011, 8, 5, 23, 59, 59);
            var actual = target.GetFollowingClose();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetFollowingMonthOpenTest()
        {
            var target = new DateTime(2011, 8, 5);
            
            var expected = new DateTime(2011, 9, 1);
            var actual = target.GetFollowingMonthlyOpen();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetFollowingMonthCloseTest()
        {
            var target = new DateTime(2011, 8, 5);
            
            var expected = new DateTime(2011, 9, 30).GetFollowingClose();
            var actual = target.GetFollowingMonthlyClose();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetMostRecentMonthOpenTest()
        {
            var target = new DateTime(2011, 8, 5);
            
            var expected = new DateTime(2011, 8, 1);
            var actual = target.GetMostRecentMonthlyOpen();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetMostRecentWeeklyOpenWeekdayTest()
        {
            var target = new DateTime(2011, 12, 22);
            
            var expected = new DateTime(2011, 12, 19);
            var actual = target.GetMostRecentWeeklyOpen();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetMostRecentWeeklyOpenSaturdayTest()
        {
            var target = new DateTime(2011, 12, 17);
            
            var expected = new DateTime(2011, 12, 12);
            var actual = target.GetMostRecentWeeklyOpen();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetMostRecentWeeklyOpenSundayTest()
        {
            var target = new DateTime(2011, 12, 18);
            
            var expected = new DateTime(2011, 12, 12);
            var actual = target.GetMostRecentWeeklyOpen();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetMostRecentWeeklyCloseWeekdayTest()
        {
            var target = new DateTime(2011, 12, 20);

            var expected = new DateTime(2011, 12, 16).GetFollowingClose();
            var actual = target.GetMostRecentWeeklyClose();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetMostRecentWeeklyCloseSaturdayTest()
        {
            var target = new DateTime(2011, 12, 17);

            var expected = new DateTime(2011, 12, 16).GetFollowingClose();
            var actual = target.GetMostRecentWeeklyClose();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetMostRecentWeeklyCloseSundayTest()
        {
            var target = new DateTime(2011, 12, 18);

            var expected = new DateTime(2011, 12, 16).GetFollowingClose();
            var actual = target.GetMostRecentWeeklyClose();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddPeriodHourTest()
        {
            var target = new DateTime(2012, 1, 19);
            const Resolution resolution = Resolution.Hours;

            var expected = target.AddHours(1);
            var actual = target.AddPeriod(resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddPeriodDayTest()
        {
            var target = new DateTime(2012, 1, 19);
            const Resolution resolution = Resolution.Days;

            var expected = target.GetFollowingOpen();
            var actual = target.AddPeriod(resolution);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddPeriodWeekTest()
        {
            var target = new DateTime(2012, 1, 19);
            const Resolution resolution = Resolution.Weeks;

            var expected = new DateTime(2012, 1, 26);
            var actual = target.AddPeriod(resolution);
            Assert.AreEqual(expected, actual);
        }
    }
}

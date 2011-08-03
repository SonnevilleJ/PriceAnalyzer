using Sonneville.PriceTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
        public void GetNextOpenTestFromMidnight()
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
        public void GetNextOpenTestFromEndOfDay()
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
        public void GetNextOpenTestFromSaturday()
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
        public void GetNextCloseTestFromNoon()
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
        public void GetNextCloseTestFromEndOfDay()
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
        public void GetNextCloseTestFromSaturday()
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
        public void GetFollowingWeekCloseTestFromFriday()
        {
            var dateTime = new DateTime(2011, 8, 5);
            var expected = new DateTime(2011, 8, 5, 23, 59, 59);
            
            var actual = DateTimeExtensions.GetFollowingWeekClose(dateTime);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetFollowingWeekOpen
        /// </summary>
        [TestMethod]
        public void GetFollowingWeekOpenTestFromWednesday()
        {
            var dateTime = new DateTime(2011, 8, 2);
            var expected = new DateTime(2011, 8, 8);
            
            var actual = DateTimeExtensions.GetFollowingWeekOpen(dateTime);
            
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetFollowingWeekOpen
        /// </summary>
        [TestMethod]
        public void GetFollowingWeekOpenTestFromMonday()
        {
            var dateTime = new DateTime(2011, 8, 8);
            var expected = new DateTime(2011, 8, 15);
            
            var actual = DateTimeExtensions.GetFollowingWeekOpen(dateTime);
            
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnsureWeekdayTestMonday()
        {
            var dateTime = new DateTime(2011, 8, 1);
            var expected = dateTime;

            var actual = DateTimeExtensions.EnsureWeekday(dateTime);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnsureWeekdayTestTuesday()
        {
            var dateTime = new DateTime(2011, 8, 2);
            var expected = dateTime;

            var actual = DateTimeExtensions.EnsureWeekday(dateTime);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnsureWeekdayTestWednesday()
        {
            var dateTime = new DateTime(2011, 8, 3);
            var expected = dateTime;

            var actual = DateTimeExtensions.EnsureWeekday(dateTime);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnsureWeekdayTestThursday()
        {
            var dateTime = new DateTime(2011, 8, 4);
            var expected = dateTime;

            var actual = DateTimeExtensions.EnsureWeekday(dateTime);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnsureWeekdayTestFriday()
        {
            var dateTime = new DateTime(2011, 8, 5);
            var expected = dateTime;

            var actual = DateTimeExtensions.EnsureWeekday(dateTime);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnsureWeekdayTestSaturday()
        {
            var dateTime = new DateTime(2011, 8, 6);
            var expected = new DateTime(2011, 8, 8);

            var actual = DateTimeExtensions.EnsureWeekday(dateTime);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnsureWeekdayTestSunday()
        {
            var dateTime = new DateTime(2011, 8, 7);
            var expected = new DateTime(2011, 8, 8);

            var actual = DateTimeExtensions.EnsureWeekday(dateTime);

            Assert.AreEqual(expected, actual);
        }
    }
}

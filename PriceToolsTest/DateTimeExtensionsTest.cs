﻿using Sonneville.PriceTools;
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
        /// A test for GetNextOpen
        /// </summary>
        [TestMethod]
        public void GetNextOpenTestFromMidnight()
        {
            var dateTime = new DateTime(2011, 8, 2);
            var expected = new DateTime(2011, 8, 3);
            
            var actual = DateTimeExtensions.GetNextOpen(dateTime);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetNextOpen
        /// </summary>
        [TestMethod]
        public void GetNextOpenTestFromEndOfDay()
        {
            var dateTime = new DateTime(2011, 8, 2, 23, 59, 59);
            var expected = new DateTime(2011, 8, 3);
            
            var actual = DateTimeExtensions.GetNextOpen(dateTime);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetNextOpen
        /// </summary>
        [TestMethod]
        public void GetNextOpenTestFromSaturday()
        {
            var dateTime = new DateTime(2011, 8, 6);
            var expected = new DateTime(2011, 8, 8);

            var actual = DateTimeExtensions.GetNextOpen(dateTime);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetNextClose
        /// </summary>
        [TestMethod]
        public void GetNextCloseTestFromNoon()
        {
            var dateTime = new DateTime(2011, 8, 2, 12, 0, 0);
            var expected = new DateTime(2011, 8, 2, 23, 59, 59);
            
            var actual = DateTimeExtensions.GetNextClose(dateTime);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetNextClose
        /// </summary>
        [TestMethod]
        public void GetNextCloseTestFromEndOfDay()
        {
            var dateTime = new DateTime(2011, 8, 2, 23, 59, 59);
            var expected = new DateTime(2011, 8, 3, 23, 59, 59);
            
            var actual = DateTimeExtensions.GetNextClose(dateTime);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetNextClose
        /// </summary>
        [TestMethod]
        public void GetNextCloseTestFromSaturday()
        {
            var dateTime = new DateTime(2011, 8, 6);
            var expected = new DateTime(2011, 8, 8, 23, 59, 59);
            
            var actual = DateTimeExtensions.GetNextClose(dateTime);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetNextWeekClose
        /// </summary>
        [TestMethod]
        public void GetNextWeekCloseTestFromWednesday()
        {
            var dateTime = new DateTime(2011, 8, 2);
            var expected = new DateTime(2011, 8, 5, 23, 59, 59);
            
            var actual = DateTimeExtensions.GetNextWeekClose(dateTime);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetNextWeekClose
        /// </summary>
        [TestMethod]
        public void GetNextWeekCloseTestFromFriday()
        {
            var dateTime = new DateTime(2011, 8, 5);
            var expected = new DateTime(2011, 8, 5, 23, 59, 59);
            
            var actual = DateTimeExtensions.GetNextWeekClose(dateTime);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetNextWeekOpen
        /// </summary>
        [TestMethod]
        public void GetNextWeekOpenTestFromWednesday()
        {
            var dateTime = new DateTime(2011, 8, 2);
            var expected = new DateTime(2011, 8, 8);
            
            var actual = DateTimeExtensions.GetNextWeekOpen(dateTime);
            
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetNextWeekOpen
        /// </summary>
        [TestMethod]
        public void GetNextWeekOpenTestFromMonday()
        {
            var dateTime = new DateTime(2011, 8, 8);
            var expected = new DateTime(2011, 8, 15);
            
            var actual = DateTimeExtensions.GetNextWeekOpen(dateTime);
            
            Assert.AreEqual(expected, actual);
        }
    }
}

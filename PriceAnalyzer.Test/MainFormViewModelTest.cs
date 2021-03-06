﻿using System.IO;
using NUnit.Framework;
using Sonneville.PriceTools.SampleData;

namespace Sonneville.PriceTools.PriceAnalyzer.Test
{
    [TestFixture]
    public class MainFormViewModelTest
    {
        [Test]
        public void TickerShouldBeAssignedAfterOpeningFile()
        {
            var csvString = SamplePriceDatas.Deere.CsvString;
            var tempFileName = Path.GetTempFileName();
            File.WriteAllText(tempFileName, csvString);

            var viewModel = new MainFormViewModel();
            viewModel.OpenFile(tempFileName);

            var actual = viewModel.Ticker;
            var expected = new FileInfo(tempFileName).Name.Replace(".tmp", "");
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TickerShouldBeEmptyBeforeOpeningFile()
        {
            var viewModel = new MainFormViewModel();
            var actual = viewModel.Ticker;

            Assert.IsTrue(string.IsNullOrEmpty(actual));
        }
    }
}
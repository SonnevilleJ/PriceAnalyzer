﻿using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.Statistics.Test
{
    [TestClass]
    public class DistributionTest
    {
        private Distribution _distribution;
        private Random _random;

        [TestInitialize]
        public void Initialize()
        {
            _distribution = new Distribution();
            _random = new Random();
        }

        [TestMethod]
        public void ExcelTest1()
        {
            var actual = _distribution.FindInverseNormal(.5, 8, 18);

            Assert.AreEqual(8, actual);
        }

        [TestMethod]
        public void ExcelTest2()
        {
            var actual = _distribution.FindInverseNormal(.04, .04, 1985);

            Assert.AreEqual(-3475.07185143556, actual, 0.00000000001);
        }

        [TestMethod]
        public void RandomMeanTest()
        {
            var mean = _random.Next();
            var actual = _distribution.FindInverseNormal(.5, mean, 18);

            Assert.AreEqual(mean, actual);
        }

        [TestMethod]
        public void RandomMeanAndStandardDeviationTest()
        {
            var mean = _random.Next();
            var standardDeviation = _random.Next();
            var actual = _distribution.FindInverseNormal(.5, mean, standardDeviation);

            Assert.AreEqual(mean, actual);
        }

        /// <summary>
        /// Runs many iterations, designed to weed out performance issues and not necessarily threading issues.
        /// Executes in under 4 seconds on 3.4 GHz quad core CPU.
        /// </summary>
        [TestMethod]
        public void ParallelTest()
        {
            const int iterations = 100000;
            var tasks = new Task[iterations];
            for (var i = 0; i < iterations; i++)
            {
                var task = new Task(() =>
                {
                    for (var j = 0; j < 100; j++)
                    {
                        RandomMeanAndStandardDeviationTest();
                    }
                });
                tasks[i] = task;
                task.Start();
            }
            Task.WaitAll(tasks);
        }
    }
}
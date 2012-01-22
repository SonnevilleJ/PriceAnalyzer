using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Extensions;
using Sonneville.PriceTools.SamplePriceData;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class ReactionMovesTest
    {
        private static IEnumerable<ReactionMove> ExpectedReactionHighs
        {
            get
            {
                return new List<ReactionMove>
                           {
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 1, 13), HighLow = HighLow.High, Reaction = 89.97m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 1, 18), HighLow = HighLow.High, Reaction = 91.63m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 1, 21), HighLow = HighLow.High, Reaction = 90.64m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 2, 2), HighLow = HighLow.High, Reaction = 94.24m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 2, 7), HighLow = HighLow.High, Reaction = 94.61m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 2, 9), HighLow = HighLow.High, Reaction = 94.74m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 2, 14), HighLow = HighLow.High, Reaction = 95.9m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 2, 16), HighLow = HighLow.High, Reaction = 97.36m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 2, 25), HighLow = HighLow.High, Reaction = 92m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 3, 3), HighLow = HighLow.High, Reaction = 93.16m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 3, 14), HighLow = HighLow.High, Reaction = 88.13m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 3, 21), HighLow = HighLow.High, Reaction = 92.46m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 3, 28), HighLow = HighLow.High, Reaction = 94.98m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 4, 1), HighLow = HighLow.High, Reaction = 99.8m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 4, 15), HighLow = HighLow.High, Reaction = 94.49m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 4, 26), HighLow = HighLow.High, Reaction = 97.78m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 5, 2), HighLow = HighLow.High, Reaction = 98.3m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 5, 6), HighLow = HighLow.High, Reaction = 94.17m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 5, 10), HighLow = HighLow.High, Reaction = 94.61m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 5, 19), HighLow = HighLow.High, Reaction = 87.73m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 6, 9), HighLow = HighLow.High, Reaction = 82.79m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 6, 14), HighLow = HighLow.High, Reaction = 82.66m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 6, 22), HighLow = HighLow.High, Reaction = 83m},
                           };
            }
        }

        private static IEnumerable<ReactionMove> ExpectedReactionLows
        {
            get
            {
                return new List<ReactionMove>
                           {
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 1, 4), HighLow = HighLow.Low, Reaction = 81.8m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 1, 20), HighLow = HighLow.Low, Reaction = 86.89m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 1, 25), HighLow = HighLow.Low, Reaction = 88.24m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 1, 31), HighLow = HighLow.Low, Reaction = 88.38m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 2, 3), HighLow = HighLow.Low, Reaction = 92.37m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 2, 8), HighLow = HighLow.Low, Reaction = 93.01m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 2, 15), HighLow = HighLow.Low, Reaction = 93.09m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 2, 23), HighLow = HighLow.Low, Reaction = 86.23m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 3, 1), HighLow = HighLow.Low, Reaction = 88.33m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 3, 8), HighLow = HighLow.Low, Reaction = 88.9m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 3, 11), HighLow = HighLow.Low, Reaction = 84.59m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 3, 15), HighLow = HighLow.Low, Reaction = 84.27m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 3, 23), HighLow = HighLow.Low, Reaction = 90.12m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 3, 29), HighLow = HighLow.Low, Reaction = 91.75m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 4, 14), HighLow = HighLow.Low, Reaction = 92.61m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 4, 18), HighLow = HighLow.Low, Reaction = 90.65m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 4, 27), HighLow = HighLow.Low, Reaction = 94.65m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 5, 5), HighLow = HighLow.Low, Reaction = 91m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 5, 18), HighLow = HighLow.Low, Reaction = 84.65m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 5, 23), HighLow = HighLow.Low, Reaction = 82.25m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 6, 1), HighLow = HighLow.Low, Reaction = 82.73m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 6, 8), HighLow = HighLow.Low, Reaction = 79.61m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 6, 13), HighLow = HighLow.Low, Reaction = 79.77m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 6, 16), HighLow = HighLow.Low, Reaction = 78.23m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 6, 20), HighLow = HighLow.Low, Reaction = 77.81m},
                               new ReactionMove
                                   {DateTime = new DateTime(2011, 6, 23), HighLow = HighLow.Low, Reaction = 78.8m}
                           };
            }
        }

        private static IEnumerable<ReactionMove> ExpectedReactionMoves
        {
            get
            {
                var reactionMoves = ExpectedReactionHighs.ToList();
                reactionMoves.AddRange(ExpectedReactionLows);
                reactionMoves.OrderBy(rm => rm.DateTime);

                return reactionMoves;
            }
        }

        [TestMethod]
        public void ReactionMovesCountTest()
        {
            var reactionMoves = ExpectedReactionMoves;
            var target = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;

            var actualMoves = target.GetReactionMoves();

            Assert.AreEqual(reactionMoves.Count(), actualMoves.Count());
        }

        [TestMethod]
        public void ReactionMovesValuesTest()
        {
            var reactionMoves = ExpectedReactionMoves;
            var target = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;

            var actualMoves = target.GetReactionMoves();

            foreach (var reactionMove in reactionMoves)
            {
                Assert.IsTrue(actualMoves.Contains(reactionMove));
            }
        }

        [TestMethod]
        public void ReactionHighsCountTest()
        {
            var newHighs = ExpectedReactionHighs;
            var target = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;

            var actualHighs = target.GetReactionHighs();

            Assert.AreEqual(newHighs.Count(), actualHighs.Count());
        }

        [TestMethod]
        public void ReactionHighsTest()
        {
            var newHighs = ExpectedReactionHighs;
            var target = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;

            var actualHighs = target.GetReactionHighs();

            foreach (var reactionMove in newHighs)
            {
                Assert.IsTrue(actualHighs.Contains(reactionMove));
            }
        }

        [TestMethod]
        public void ReactionLowsCountTest()
        {
            var newLows = ExpectedReactionLows;
            var target = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;

            var actualLows = target.GetReactionLows();

            Assert.AreEqual(newLows.Count(), actualLows.Count());
        }

        [TestMethod]
        public void ReactionLowsTest()
        {
            var newLows = ExpectedReactionLows;
            var target = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;

            var actualLows = target.GetReactionLows();

            foreach (var reactionMove in newLows)
            {
                Assert.IsTrue(actualLows.Contains(reactionMove));
            }
        }
    }
}

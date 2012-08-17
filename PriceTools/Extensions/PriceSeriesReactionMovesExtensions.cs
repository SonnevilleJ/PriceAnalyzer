﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Sonneville.PriceTools.Extensions
{
    /// <summary>
    /// Contains extension methods for calculating reaction moves.
    /// </summary>
    public static class PriceSeriesReactionMovesExtensions
    {
        /// <summary>
        /// Gets a collection of reaction moves observed in the PriceSeries.
        /// </summary>
        public static IEnumerable<ReactionMove> GetReactionMoves(this PriceSeries priceSeries)
        {
            if (priceSeries == null) throw new ArgumentNullException("priceSeries", Strings.PriceSeriesReactionMovesExtensions_GetReactionMoves_Parameter_priceSeries_cannot_be_null_);

            var moves = new List<ReactionMove>();

            var todayUp = false;
            var todayDown = false;
            var yesterdayUp = false;
            var yesterdayDown = false;

            var pricePeriods = priceSeries.PricePeriods;
            for (var i = 1; i < pricePeriods.Count; i++)
            {
                var yesterday = pricePeriods[i - 1];
                var today = pricePeriods[i];
                if (i > 1)
                {
                    yesterdayUp = todayUp;
                    yesterdayDown = todayDown;
                    //yesterdayConverging = todayConverging;
                    //yesterdayWidening = todayWidening;
                }

                // calculate change
                var highChange = today.High - yesterday.High;
                var higherHigh = highChange > 0;
                var lowerHigh = highChange < 0;

                var lowChange = today.Low - yesterday.Low;
                var higherLow = lowChange > 0;
                var lowerLow = lowChange < 0;

                // calculate direction
                todayUp = higherHigh && higherLow;
                todayDown = lowerHigh && lowerLow;
                //todayConverging = ((lowerHigh && !lowerLow) || (higherLow && !higherHigh));
                var todayWidening = higherHigh && lowerLow;

                if (i > 1)
                {
                    if (yesterdayUp && !todayUp && !todayWidening)
                        moves.Add(new ReactionMove {DateTime = yesterday.Head, HighLow = HighLow.High, Reaction = yesterday.High});
                    if (yesterdayDown && !todayDown && !todayWidening)
                        moves.Add(new ReactionMove {DateTime = yesterday.Head, HighLow = HighLow.Low, Reaction = yesterday.Low});
                }
            }
            return moves;
        }

        /// <summary>
        /// Gets a collection of reaction highs observed in the PriceSeries.
        /// </summary>
        public static IEnumerable<ReactionMove> GetReactionHighs(this PriceSeries priceSeries)
        {
            return priceSeries.GetReactionMoves().Where(rm => rm.HighLow == HighLow.High);
        }

        /// <summary>
        /// Gets a collection of reaction lows observed in the PriceSeries.
        /// </summary>
        public static IEnumerable<ReactionMove> GetReactionLows(this PriceSeries priceSeries)
        {
            return priceSeries.GetReactionMoves().Where(rm => rm.HighLow == HighLow.Low);
        }
    }
}

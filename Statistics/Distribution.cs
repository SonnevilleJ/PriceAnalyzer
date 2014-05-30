using MathNet.Numerics.Distributions;

namespace Sonneville.Statistics
{
    public class Distribution
    {
        public double FindInverseNormal(double probability, double mean, double standardDeviation)
        {
            return new Normal(mean, standardDeviation).InverseCumulativeDistribution(probability);
        }
    }
}
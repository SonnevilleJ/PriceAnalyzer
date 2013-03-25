using System;
using System.Linq;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Creates repeatable <see cref="Guid"/>s based on seed objects.
    /// </summary>
    public class GuidSeeder
    {
        /// <summary>
        /// Constructs a new <see cref="Guid"/> based on the hash codes of the provided seeds.
        /// </summary>
        /// <param name="seeds">A list of seeds. GetHashCode is called on each object to affect the resulting Guid.</param>
        /// <returns>A new <see cref="Guid"/> based on the hash codes of the provided seeds.</returns>
        public Guid SeedGuid(params object[] seeds)
        {
            var hashCode = 0;
            unchecked
            {
                hashCode = seeds.Aggregate(hashCode, (current, seed) => (current*397) ^ seed.GetHashCode());
            }
            var random = new Random(hashCode);
            var guid = new byte[16];
            random.NextBytes(guid);

            return new Guid(guid);
        }
    }
}
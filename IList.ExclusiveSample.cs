// A part of the C# Language Syntactic Sugar suite.

using System;
using System.Collections.Generic;

namespace CLSS
{
  public static partial class IListExclusiveSample
  {
    /// <summary>
    /// Randomly samples a number of non-repeating elements from the source.
    /// It uses a shared <see cref="Random"/> instance of the CLSS suite for
    /// rolling by default. If a custom-seeded <see cref="Random"/> instance
    /// is provided, it will be used for rolling instead.
    /// </summary>
    /// <typeparam name="T">The type of the elements of
    /// <paramref name="source"/>.</typeparam>
    /// <param name="source">The list of elements to sample.</param>
    /// <param name="sampleNumber">The number of elements to sample.</param>
    /// <param name="rng">Optional custom-seeded random number generator to use
    /// for the sample rolls.</param>
    /// <returns>An array of sampled elements.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is
    /// null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="sampleNumber"/> is larger than the size of
    /// <paramref name="source"/>.</exception>
    public static T[] ExclusiveSample<T>(this IList<T> source,
      int sampleNumber,
      Random rng = null)
    {
      if (source == null) throw new ArgumentNullException("source");
      if (sampleNumber > source.Count)
        throw new ArgumentOutOfRangeException("Cannot sample more elements than what the source collection contains");
      if (rng == null) rng = DefaultRandom.Instance;
      var results = new T[sampleNumber];
      int resultIndex = 0;
      for (int i = 0; i < source.Count && resultIndex < sampleNumber; ++i)
      {
        var probability = (double)(sampleNumber - resultIndex)
          / (source.Count - i);
        if (rng.NextDouble() < probability)
        { results[resultIndex] = source[i]; ++resultIndex; }
      }
      return results;
    }
  }
}
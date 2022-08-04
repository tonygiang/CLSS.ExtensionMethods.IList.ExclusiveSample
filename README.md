# CLSS.ExtensionMethods.IList.ExclusiveSample

### Problem

Randomly selecting an element out of a list is a simple operation. So is selecting multiple repeatable elements out of a list. But what if you want to randomly select `k` number of non-repeating elements out of a list? This operation is called "exclusive sampling" here to hint at its non-repeating nature.

Naively, you can write `elements.OrderBy(e => rng.Next()).Take(k)`, but this has a time complexity of O(n log n). Alternatively, you can also replace the `.OrderBy(e => rng.Next())` with a [Fisher-Yates shuffle](https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle) to achieve a time complexity of O(n). This algorithm still has some problems:

- It will mutate the source list, necessitating an allocation of its clone - thus a space complexity of O(n) - if you want to leave the source list untampered.
- Performing a Fisher-Yates shuffle will also iterate through the entire list in all cases, hence the complexity is O(n) even in the best-case scenario.
- The sampled results will not be in-order, which may not be desirable for your use-cases.

### Solution

In The Art of Computer Programming Volume 2, Donald Knuth introduced the following algorithm:

**Algorithm S** (*Selection sampling technique*). To select *n* records at random from a set of *N*, where 0 < *n* ≤ *N*.

**S1.** [Initialize.] Set *t* ← 0, *m* ← 0. (During this algorithm, *m* represents the number of records selected so far, and *t* is the total number of input records that we have dealt with.)

**S2.** [Generate *U*.] Generate a random number *U*, uniformly distributed between zero and one.

**S3.** [Test.] If (*N* - *t*)*U* ≥ *n* - *m*, go to step S5.

**S4.** [Select.] Select the next record for the sample, and increase *m* and *t* by 1. If *m* < *t*, go to step S2; otherwise the sample is complete and the algorithm terminates.

**S5.** [Skip.] Skip the next record (do not include it in the sample), increase *t* by 1, and go back to step S2.

`ExclusiveSample` is an implementation of the above algorithm for all [`IList<T>`](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ilist-1) types. It has a time complexity of O(n) only in the worst-case and a space complexity of O(1). It does not mutate the source list. It returns sampled results in-order.

This method optionally takes in an existing `System.Random` instance which is seeded with a custom value you would prefer for the sample. Otherwise, it rolls for a random number using the CLSS's shared `DefaultRandom` instance. If you are on .NET 6, you can pass in [`System.Random.Shared`](https://docs.microsoft.com/en-us/dotnet/api/system.random.shared).

**Note**: `ExclusiveSample` works on all types implementing the [`IList<T>`](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ilist-1) interface, *including raw C# array*.

##### This package is a part of the [C# Language Syntactic Sugar suite](https://github.com/tonygiang/CLSS).
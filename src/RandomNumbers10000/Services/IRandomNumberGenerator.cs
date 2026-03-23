namespace RandomNumbers10000.Services;

/// <summary>
/// Defines the contract for generating a list of unique random numbers within a specified range.
/// </summary>
public interface IRandomNumberGenerator
{
    /// <summary>
    /// Generates a list of unique random numbers using the Fisher-Yates shuffle algorithm.
    /// </summary>
    /// <param name="count">The total count of numbers to generate.</param>
    /// <param name="minValue">The minimum value (inclusive) for the range.</param>
    /// <param name="maxValue">The maximum value (inclusive) for the range.</param>
    /// <param name="seed">Optional seed for reproducible results. If null, uses a random seed.</param>
    /// <returns>A list of unique random numbers in the specified range.</returns>
    /// <remarks>
    /// Uses the Fisher-Yates shuffle algorithm for optimal O(n) time complexity and O(n) space complexity.
    /// https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
    /// This ensures all numbers are unique and randomly distributed.
    /// </remarks>
    IReadOnlyList<int> GenerateUniqueRandomNumbers(int count, int minValue, int maxValue, int? seed = null);
}


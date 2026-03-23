namespace RandomNumbers10000.Services;

using Microsoft.Extensions.Logging;

/// <summary>
/// Generates a list of unique random numbers using the Fisher-Yates shuffle algorithm.
/// The idea is to generate a simple 1-to N sequence and then shuffle it.
/// </summary>
public class RandomNumberGenerator : IRandomNumberGenerator
{
    private readonly ILogger<RandomNumberGenerator> _logger;


    /// <summary>
    /// Initializes a new instance of the <see cref="RandomNumberGenerator"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for diagnostic logging.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="logger"/> is null.</exception>
    public RandomNumberGenerator(ILogger<RandomNumberGenerator> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


    /// <inheritdoc />
    public IReadOnlyList<int> GenerateUniqueRandomNumbers(int count, int minValue, int maxValue, int? seed = null)
    {
        ValidateParameters(count, minValue, maxValue);

        _logger.LogInformation("Starting generation of {Count} unique random numbers between {MinValue} and {MaxValue}", count, minValue, maxValue);

        var random = seed.HasValue ? new Random(seed.Value) : new Random();
        var numbers = CreateInitialSequence(minValue, maxValue);

        FisherYatesShuffle(numbers, random);

        var result = numbers.Take(count).ToList();

        _logger.LogInformation("Successfully generated {Count} unique random numbers", count);

        return result.AsReadOnly();
    }


    /// <summary>
    /// Validates the input parameters for generating random numbers.
    /// </summary>
    /// <param name="count">The count of numbers to generate.</param>
    /// <param name="minValue">The minimum value of the range.</param>
    /// <param name="maxValue">The maximum value of the range.</param>
    /// <exception cref="ArgumentException">Thrown when parameters are invalid.</exception>
    private static void ValidateParameters(int count, int minValue, int maxValue)
    {
        if (count <= 0)
        {
            throw new ArgumentException("Count must be greater than zero.", nameof(count));
        }

        if (minValue > maxValue)
        {
            throw new ArgumentException($"Minimum value ({minValue}) cannot be greater than maximum value ({maxValue}).", nameof(minValue));
        }

        var rangeSize = maxValue - minValue + 1;
        if (count > rangeSize)
        {
            throw new ArgumentException($"Count ({count}) cannot exceed the range size ({rangeSize}).", nameof(count));
        }
    }


    /// <summary>
    /// Creates an initial sequence of numbers from minValue to maxValue (inclusive). This is just simple for loop
    /// </summary>
    /// <param name="minValue">The starting value of the sequence.</param>
    /// <param name="maxValue">The ending value of the sequence.</param>
    /// <returns>A list containing all numbers in the range [minValue, maxValue].</returns>
    private static List<int> CreateInitialSequence(int minValue, int maxValue)
    {
        var numbers = new List<int>(maxValue - minValue + 1);
        for (int i = minValue; i <= maxValue; i++)
        {
            numbers.Add(i);
        }

        return numbers;
    }


    /// <summary>
    /// Performs the Fisher-Yates shuffle algorithm on the provided list.
    /// </summary>
    /// <param name="numbers">The list to shuffle. This list is modified in-place.</param>
    /// <param name="random">The random number generator to use for shuffling.</param>
    /// <remarks>
    /// Fisher-Yates shuffle provides O(n) time complexity and ensures uniform distribution. 
    /// Algorithm: For each position from end to start, swap with a random position before or equal to current.
    /// </remarks>
    private static void FisherYatesShuffle(List<int> numbers, Random random)
    {
        for (int i = numbers.Count - 1; i > 0; i--)
        {
            // this the core of the algorithm, generating the random index to swap with the current index. 
            int randomIndex = random.Next(0, i + 1);

            // Swap
            (numbers[i], numbers[randomIndex]) = (numbers[randomIndex], numbers[i]);
        }
    }
}


namespace RandomNumbers10000.Tests.Services;

using Microsoft.Extensions.Logging;
using Moq;
using RandomNumbers10000.Services;
using Xunit;

/// <summary>
/// Unit tests for the <see cref="RandomNumberGenerator"/> service.
/// </summary>
public class RandomNumberGeneratorTests
{
    private readonly Mock<ILogger<RandomNumberGenerator>> _mockLogger;
    private readonly RandomNumberGenerator _generator;


    /// <summary>
    /// Initializes a new instance of the <see cref="RandomNumberGeneratorTests"/> class.
    /// </summary>
    public RandomNumberGeneratorTests()
    {
        _mockLogger = new Mock<ILogger<RandomNumberGenerator>>();
        _generator = new RandomNumberGenerator(_mockLogger.Object);
    }


    /// <summary>
    /// Test: Verify that the generator produces the correct count of numbers.
    /// </summary>
    [Fact]
    public void GenerateUniqueRandomNumbers_WithValidParameters_ReturnsCorrectCount()
    {
        // Arrange
        const int count = 100;
        const int minValue = 1;
        const int maxValue = 100;

        // Act
        var result = _generator.GenerateUniqueRandomNumbers(count, minValue, maxValue);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(count, result.Count);
    }


    /// <summary>
    /// Test: Verify that all generated numbers are within the specified range.
    /// </summary>
    [Fact]
    public void GenerateUniqueRandomNumbers_AllNumbersAreInRange()
    {
        // Arrange
        const int count = 1000;
        const int minValue = 1;
        const int maxValue = 10_000;

        // Act
        var result = _generator.GenerateUniqueRandomNumbers(count, minValue, maxValue);

        // Assert
        foreach (var number in result)
        {
            Assert.InRange(number, minValue, maxValue);
        }
    }


    /// <summary>
    /// Test: Verify that all generated numbers are unique.
    /// </summary>
    [Fact]
    public void GenerateUniqueRandomNumbers_AllNumbersAreUnique()
    {
        // Arrange
        const int count = 5000;
        const int minValue = 1;
        const int maxValue = 10_000;

        // Act
        var result = _generator.GenerateUniqueRandomNumbers(count, minValue, maxValue);

        // Assert
        var uniqueNumbers = new HashSet<int>(result);
        Assert.Equal(count, uniqueNumbers.Count);
    }


    /// <summary>
    /// Test: Verify reproducibility with a fixed seed.
    /// </summary>
    [Fact]
    public void GenerateUniqueRandomNumbers_WithFixedSeed_ProducesReproducibleResults()
    {
        // Arrange
        const int count = 100;
        const int minValue = 1;
        const int maxValue = 100;
        const int seed = 42;

        // Act
        var result1 = _generator.GenerateUniqueRandomNumbers(count, minValue, maxValue, seed);
        var result2 = _generator.GenerateUniqueRandomNumbers(count, minValue, maxValue, seed);

        // Assert
        Assert.Equal(result1, result2);
    }


    /// <summary>
    /// Test: Verify that different seeds produce different results.
    /// </summary>
    [Fact]
    public void GenerateUniqueRandomNumbers_WithDifferentSeeds_ProducesDifferentResults()
    {
        // Arrange
        const int count = 100;
        const int minValue = 1;
        const int maxValue = 100;
        const int seed1 = 42;
        const int seed2 = 123;

        // Act
        var result1 = _generator.GenerateUniqueRandomNumbers(count, minValue, maxValue, seed1);
        var result2 = _generator.GenerateUniqueRandomNumbers(count, minValue, maxValue, seed2);

        // Assert
        // While statistically possible to be equal, highly unlikely for 100 numbers
        Assert.NotEqual(result1, result2);
    }


    /// <summary>
    /// Test: Verify the main scenario - generating 10,000 unique numbers.
    /// </summary>
    [Fact]
    public void GenerateUniqueRandomNumbers_Generate10000Numbers_Success()
    {
        // Arrange
        const int count = 10_000;
        const int minValue = 1;
        const int maxValue = 10_000;

        // Act
        var result = _generator.GenerateUniqueRandomNumbers(count, minValue, maxValue);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(count, result.Count);

        // Verify uniqueness
        var uniqueNumbers = new HashSet<int>(result);
        Assert.Equal(count, uniqueNumbers.Count);

        // Verify all numbers are in range
        foreach (var number in result)
        {
            Assert.InRange(number, minValue, maxValue);
        }
    }


    /// <summary>
    /// Test: Verify boundary condition - generating exactly the range size.
    /// </summary>
    [Fact]
    public void GenerateUniqueRandomNumbers_GenerateFullRange_Success()
    {
        // Arrange
        const int count = 50;
        const int minValue = 1;
        const int maxValue = 50;

        // Act
        var result = _generator.GenerateUniqueRandomNumbers(count, minValue, maxValue);

        // Assert
        Assert.Equal(count, result.Count);
        var uniqueNumbers = new HashSet<int>(result);
        Assert.Equal(count, uniqueNumbers.Count);
        Assert.True(result.All(n => n >= minValue && n <= maxValue));
    }


    /// <summary>
    /// Test: Verify that count must be greater than zero.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void GenerateUniqueRandomNumbers_InvalidCount_ThrowsArgumentException(int invalidCount)
    {
        // Arrange
        const int minValue = 1;
        const int maxValue = 100;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _generator.GenerateUniqueRandomNumbers(invalidCount, minValue, maxValue));

        Assert.Contains("Count must be greater than zero", exception.Message);
    }


    /// <summary>
    /// Test: Verify that minValue cannot be greater than maxValue.
    /// </summary>
    [Fact]
    public void GenerateUniqueRandomNumbers_MinGreaterThanMax_ThrowsArgumentException()
    {
        // Arrange
        const int count = 100;
        const int minValue = 100;
        const int maxValue = 50;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _generator.GenerateUniqueRandomNumbers(count, minValue, maxValue));

        Assert.Contains("cannot be greater than maximum", exception.Message);
    }


    /// <summary>
    /// Test: Verify that count cannot exceed the range size.
    /// </summary>
    [Fact]
    public void GenerateUniqueRandomNumbers_CountExceedsRange_ThrowsArgumentException()
    {
        // Arrange
        const int count = 150;
        const int minValue = 1;
        const int maxValue = 100;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _generator.GenerateUniqueRandomNumbers(count, minValue, maxValue));

        Assert.Contains("cannot exceed the range size", exception.Message);
    }


    /// <summary>
    /// Test: Verify edge case with single number generation.
    /// </summary>
    [Fact]
    public void GenerateUniqueRandomNumbers_GenerateSingleNumber_Success()
    {
        // Arrange
        const int count = 1;
        const int minValue = 1;
        const int maxValue = 100;

        // Act
        var result = _generator.GenerateUniqueRandomNumbers(count, minValue, maxValue);

        // Assert
        Assert.Single(result);
        Assert.InRange(result[0], minValue, maxValue);
    }


    /// <summary>
    /// Test: Verify edge case with negative number ranges.
    /// </summary>
    [Fact]
    public void GenerateUniqueRandomNumbers_NegativeRange_Success()
    {
        // Arrange
        const int count = 50;
        const int minValue = -100;
        const int maxValue = -1;

        // Act
        var result = _generator.GenerateUniqueRandomNumbers(count, minValue, maxValue);

        // Assert
        Assert.Equal(count, result.Count);
        var uniqueNumbers = new HashSet<int>(result);
        Assert.Equal(count, uniqueNumbers.Count);
        Assert.True(result.All(n => n >= minValue && n <= maxValue));
    }


    /// <summary>
    /// Test: Verify edge case with mixed negative and positive numbers.
    /// </summary>
    [Fact]
    public void GenerateUniqueRandomNumbers_MixedPositiveNegativeRange_Success()
    {
        // Arrange
        const int count = 100;
        const int minValue = -50;
        const int maxValue = 50;

        // Act
        var result = _generator.GenerateUniqueRandomNumbers(count, minValue, maxValue);

        // Assert
        Assert.Equal(count, result.Count);
        var uniqueNumbers = new HashSet<int>(result);
        Assert.Equal(count, uniqueNumbers.Count);
        Assert.True(result.All(n => n >= minValue && n <= maxValue));
    }


    /// <summary>
    /// Test: Verify that the result is read-only.
    /// </summary>
    [Fact]
    public void GenerateUniqueRandomNumbers_ResultIsReadOnly()
    {
        // Arrange
        const int count = 100;
        const int minValue = 1;
        const int maxValue = 100;

        // Act
        var result = _generator.GenerateUniqueRandomNumbers(count, minValue, maxValue);

        // Assert
        Assert.IsAssignableFrom<IReadOnlyList<int>>(result);
    }


    /// <summary>
    /// Test: Verify that null logger parameter throws an exception.
    /// </summary>
    [Fact]
    public void Constructor_NullLogger_ThrowsArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new RandomNumberGenerator(null!));
        Assert.Equal("logger", exception.ParamName);
    }


    /// <summary>
    /// Test: Verify performance - generating 10,000 numbers should complete quickly.
    /// </summary>
    [Fact]
    public void GenerateUniqueRandomNumbers_Performance_CompleteInReasonableTime()
    {
        // Arrange
        const int count = 10_000;
        const int minValue = 1;
        const int maxValue = 10_000;
        const int maxMilliseconds = 5_000; // 5 seconds

        // Act
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var result = _generator.GenerateUniqueRandomNumbers(count, minValue, maxValue);
        stopwatch.Stop();

        // Assert
        Assert.Equal(count, result.Count);
        Assert.True(stopwatch.ElapsedMilliseconds < maxMilliseconds,
            $"Generation took {stopwatch.ElapsedMilliseconds}ms, expected less than {maxMilliseconds}ms");
    }
}

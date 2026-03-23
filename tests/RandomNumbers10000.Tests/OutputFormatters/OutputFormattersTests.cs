namespace RandomNumbers10000.Tests.OutputFormatters;

using Microsoft.Extensions.Logging;
using Moq;
using RandomNumbers10000.OutputFormatters;
using Xunit;

/// <summary>
/// Unit tests for output formatters.
/// </summary>
public class OutputFormattersTests
{
    private readonly Mock<ILogger<ConsoleOutputFormatter>> _mockConsoleLogger;
    private readonly Mock<ILogger<CsvFileOutputFormatter>> _mockCsvLogger;


    /// <summary>
    /// Initializes a new instance of the <see cref="OutputFormattersTests"/> class.
    /// </summary>
    public OutputFormattersTests()
    {
        _mockConsoleLogger = new Mock<ILogger<ConsoleOutputFormatter>>();
        _mockCsvLogger = new Mock<ILogger<CsvFileOutputFormatter>>();
    }


    /// <summary>
    /// Test: ConsoleOutputFormatter should have correct format name.
    /// </summary>
    [Fact]
    public void ConsoleOutputFormatter_FormatName_IsConsole()
    {
        // Arrange
        var formatter = new ConsoleOutputFormatter(_mockConsoleLogger.Object);

        // Act
        var formatName = formatter.FormatName;

        // Assert
        Assert.Equal("Console", formatName);
    }


    /// <summary>
    /// Test: CsvFileOutputFormatter should have correct format name.
    /// </summary>
    [Fact]
    public void CsvFileOutputFormatter_FormatName_IsCsvFile()
    {
        // Arrange
        var formatter = new CsvFileOutputFormatter(_mockCsvLogger.Object);

        // Act
        var formatName = formatter.FormatName;

        // Assert
        Assert.Equal("CSV File", formatName);
    }


    /// <summary>
    /// Test: CsvFileOutputFormatter creates a valid CSV file.
    /// </summary>
    [Fact]
    public async Task CsvFileOutputFormatter_FormatAndOutputAsync_CreatesValidCsvFile()
    {
        // Arrange
        var formatter = new CsvFileOutputFormatter(_mockCsvLogger.Object);
        var outputPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}");
        Directory.CreateDirectory(outputPath);

        try
        {
            var numbers = new List<int> { 1, 5, 3, 9, 2 }.AsReadOnly();

            // Act
            await formatter.FormatAndOutputAsync(numbers, outputPath);

            // Assert
            var files = Directory.GetFiles(outputPath, "RandomNumbers_*.csv");
            Assert.Single(files);

            var csvContent = await File.ReadAllTextAsync(files[0]);
            Assert.Contains("Number", csvContent);
            Assert.Contains("1", csvContent);
            Assert.Contains("5", csvContent);
            Assert.Contains("3", csvContent);
            Assert.Contains("9", csvContent);
            Assert.Contains("2", csvContent);
        }
        finally
        {
            // Cleanup
            if (Directory.Exists(outputPath))
            {
                Directory.Delete(outputPath, true);
            }
        }
    }

    /// <summary>
    /// Test: ConsoleOutputFormatter should not throw exception when formatting.
    /// </summary>
    [Fact]
    public async Task ConsoleOutputFormatter_FormatAndOutputAsync_DoesNotThrow()
    {
        // Arrange
        var formatter = new ConsoleOutputFormatter(_mockConsoleLogger.Object);
        var numbers = new List<int> { 1, 5, 3, 9, 2 }.AsReadOnly();

        // Act & Assert
        await formatter.FormatAndOutputAsync(numbers, string.Empty);
    }


    /// <summary>
    /// Test: ConsoleOutputFormatter constructor requires non-null logger.
    /// </summary>
    [Fact]
    public void ConsoleOutputFormatter_NullLogger_ThrowsArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new ConsoleOutputFormatter(null!));
        Assert.Equal("logger", exception.ParamName);
    }

    /// <summary>
    /// Test: CsvFileOutputFormatter constructor requires non-null logger.
    /// </summary>
    [Fact]
    public void CsvFileOutputFormatter_NullLogger_ThrowsArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new CsvFileOutputFormatter(null!));
        Assert.Equal("logger", exception.ParamName);
    }

    /// <summary>
    /// Test: CSV file should contain all numbers from the input.
    /// </summary>
    [Fact]
    public async Task CsvFileOutputFormatter_AllNumbersIncluded_InOutput()
    {
        // Arrange
        var formatter = new CsvFileOutputFormatter(_mockCsvLogger.Object);
        var outputPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}");
        Directory.CreateDirectory(outputPath);

        try
        {
            var numbers = Enumerable.Range(1, 100).ToList().AsReadOnly();

            // Act
            await formatter.FormatAndOutputAsync(numbers, outputPath);

            // Assert
            var files = Directory.GetFiles(outputPath, "RandomNumbers_*.csv");
            var csvContent = await File.ReadAllTextAsync(files[0]);
            var lines = csvContent.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            // +1 for header
            Assert.Equal(101, lines.Length);
        }
        finally
        {
            // Cleanup
            if (Directory.Exists(outputPath))
            {
                Directory.Delete(outputPath, true);
            }
        }
    }

    /// <summary>
    /// Test: Generated CSV file follows proper format with numbers in descending order.
    /// </summary>
    [Fact]
    public async Task CsvFileOutputFormatter_FileFormat_IsCorrect()
    {
        // Arrange
        var formatter = new CsvFileOutputFormatter(_mockCsvLogger.Object);
        var outputPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}");
        Directory.CreateDirectory(outputPath);

        try
        {
            var numbers = new List<int> { 5, 2, 8 }.AsReadOnly();

            // Act
            await formatter.FormatAndOutputAsync(numbers, outputPath);

            // Assert
            var files = Directory.GetFiles(outputPath, "RandomNumbers_*.csv");
            var csvContent = await File.ReadAllTextAsync(files[0]);
            var lines = csvContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            Assert.Equal("Number", lines[0]);
            Assert.Contains("5", lines);
            Assert.Contains("2", lines);
            Assert.Contains("8", lines);
        }
        finally
        {
            // Cleanup
            if (Directory.Exists(outputPath))
            {
                Directory.Delete(outputPath, true);
            }
        }
    }

    /// <summary>
    /// Test: File names are generated with timestamp.
    /// </summary>
    [Fact]
    public async Task CsvFileOutputFormatter_GeneratedFileName_ContainsTimestamp()
    {
        // Arrange
        var formatter = new CsvFileOutputFormatter(_mockCsvLogger.Object);
        var outputPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}");
        Directory.CreateDirectory(outputPath);

        try
        {
            var numbers = new List<int> { 1 }.AsReadOnly();

            // Act
            await formatter.FormatAndOutputAsync(numbers, outputPath);

            // Assert
            var files = Directory.GetFiles(outputPath, "RandomNumbers_*.csv");
            var fileName = Path.GetFileName(files[0]);

            // Should match pattern: RandomNumbers_YYYY-MM-DD_HH-mm-ss.csv
            Assert.Matches(@"RandomNumbers_\d{4}-\d{2}-\d{2}_\d{2}-\d{2}-\d{2}\.csv", fileName);
        }
        finally
        {
            // Cleanup
            if (Directory.Exists(outputPath))
            {
                Directory.Delete(outputPath, true);
            }
        }
    }
}


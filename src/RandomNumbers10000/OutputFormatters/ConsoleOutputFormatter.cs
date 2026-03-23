namespace RandomNumbers10000.OutputFormatters;

using Microsoft.Extensions.Logging;

/// <summary>
/// Outputs random numbers to the console with statistical summary.
/// </summary>
public class ConsoleOutputFormatter : IRandomNumberOutputFormatter
{
    private readonly ILogger<ConsoleOutputFormatter> _logger;


    /// <inheritdoc />
    public string FormatName => "Console";


    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleOutputFormatter"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for diagnostic logging.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="logger"/> is null.</exception>
    public ConsoleOutputFormatter(ILogger<ConsoleOutputFormatter> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


    /// <inheritdoc />
    public Task FormatAndOutputAsync(IReadOnlyList<int> numbers, string outputPath, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Outputting {Count} numbers to console", numbers.Count);

        Console.WriteLine();
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine($"Generated {numbers.Count:N0} Unique Random Numbers");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine();

        // Display statistics
        Console.WriteLine("📊 Statistics:");
        Console.WriteLine($"  Total Count: {numbers.Count:N0}");
        Console.WriteLine($"  Minimum: {numbers.Min():N0}");
        Console.WriteLine($"  Maximum: {numbers.Max():N0}");
        Console.WriteLine($"  Sum: {numbers.Sum():N0}");
        Console.WriteLine($"  Average: {numbers.Average():N0.00}");
        Console.WriteLine();

        // Display all numbers in a formatted table
        Console.WriteLine("📋 Numbers:");
        DisplayNumbersInTable(numbers);

        Console.WriteLine();
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("✓ Output complete!");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine();

        return Task.CompletedTask;
    }


    /// <summary>
    /// Displays numbers in a formatted table with multiple columns.
    /// </summary>
    /// <param name="numbers">The numbers to display.</param>
    private static void DisplayNumbersInTable(IReadOnlyList<int> numbers)
    {
        const int columnsPerRow = 10;
        const int columnWidth = 8;

        for (int i = 0; i < numbers.Count; i++)
        {
            if (i % columnsPerRow == 0 && i > 0)
            {
                Console.WriteLine();
            }

            Console.Write($"{numbers[i],columnWidth:D}");
        }

        Console.WriteLine();
    }
}



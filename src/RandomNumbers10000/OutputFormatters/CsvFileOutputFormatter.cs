namespace RandomNumbers10000.OutputFormatters;

using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text;

/// <summary>
/// Outputs random numbers to a timestamped CSV file.
/// </summary>
public class CsvFileOutputFormatter : IRandomNumberOutputFormatter
{
    private readonly ILogger<CsvFileOutputFormatter> _logger;


    /// <inheritdoc />
    public string FormatName => "CSV File";


    /// <summary>
    /// Initializes a new instance of the <see cref="CsvFileOutputFormatter"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for diagnostic logging.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="logger"/> is null.</exception>
    public CsvFileOutputFormatter(ILogger<CsvFileOutputFormatter> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


    /// <inheritdoc />
    public async Task FormatAndOutputAsync(IReadOnlyList<int> numbers, string outputPath, CancellationToken cancellationToken = default)
    {
        var fileName = $"RandomNumbers_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv";
        var filePath = Path.Combine(outputPath, fileName);

        _logger.LogInformation("Writing {Count} numbers to CSV file: {FilePath}", numbers.Count, filePath);

        var csv = GenerateCsv(numbers);
        await File.WriteAllTextAsync(filePath, csv, Encoding.UTF8, cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("CSV file successfully written to {FilePath}", filePath);
        Console.WriteLine($"\n✓ CSV file saved to: {filePath}\n");
    }


    /// <summary>
    /// Generates CSV content from the random numbers.
    /// </summary>
    /// <param name="numbers">The numbers to convert to CSV format.</param>
    /// <returns>CSV formatted string.</returns>
    private static string GenerateCsv(IReadOnlyList<int> numbers)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Number");

        foreach (var number in numbers)
        {
            sb.AppendLine(number.ToString(CultureInfo.InvariantCulture));
        }

        return sb.ToString();
    }
}


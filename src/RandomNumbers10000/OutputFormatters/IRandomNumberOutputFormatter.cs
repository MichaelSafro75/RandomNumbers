namespace RandomNumbers10000.OutputFormatters;

/// <summary>
/// Defines the contract for formatting and outputting random numbers in various formats.
/// </summary>
public interface IRandomNumberOutputFormatter
{
    /// <summary>
    /// Gets the name of the output format.
    /// </summary>
    string FormatName { get; }


    /// <summary>
    /// Formats and outputs the random numbers.
    /// </summary>
    /// <param name="numbers">The list of random numbers to format.</param>
    /// <param name="outputPath">The output path where the result should be saved or displayed.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task FormatAndOutputAsync(IReadOnlyList<int> numbers, string outputPath, CancellationToken cancellationToken = default);
}


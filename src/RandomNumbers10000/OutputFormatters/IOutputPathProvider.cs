namespace RandomNumbers10000.OutputFormatters;

using Microsoft.Extensions.Logging;

/// <summary>
/// Defines the contract for providing and validating output paths for file operations.
/// </summary>
public interface IOutputPathProvider
{
    /// <summary>
    /// Gets a valid output path from user interaction or default, with validation.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A valid output path where files can be written.</returns>
    Task<string> GetOutputPathAsync(CancellationToken cancellationToken = default);
}


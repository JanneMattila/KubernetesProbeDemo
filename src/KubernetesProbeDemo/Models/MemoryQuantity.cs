using System.Globalization;
using System.Text.RegularExpressions;

namespace KubernetesProbeDemo.Models;

/// <summary>
/// Parses human readable memory quantities such as "100MB", "1GB", "128Mi" or "512KiB" into a number of bytes.
/// </summary>
/// <remarks>
/// Supported format: &lt;number&gt;[prefix][i][unit]
/// <list type="bullet">
/// <item>Prefix: k, M, G or T (case insensitive).</item>
/// <item>i: indicates binary (1024 based) units, e.g. "Mi" = 1024^2. Without it units are decimal (1000 based).</item>
/// <item>Unit: "B" (bytes, default) or "b" (bits).</item>
/// </list>
/// Examples: "1024" = 1024 bytes, "100MB" = 100,000,000 bytes, "100MiB" = 104,857,600 bytes, "1Gb" = 125,000,000 bytes.
/// </remarks>
public static partial class MemoryQuantity
{
    /// <summary>
    /// Parses a human readable memory quantity into a number of bytes.
    /// </summary>
    /// <param name="value">Memory quantity, e.g. "100MB" or "1GB".</param>
    /// <returns>Number of bytes.</returns>
    /// <exception cref="FormatException">Thrown when the value cannot be parsed.</exception>
    public static long Parse(string value)
    {
        if (!TryParse(value, out var bytes))
        {
            throw new FormatException($"'{value}' is not a valid memory quantity.");
        }

        return bytes;
    }

    /// <summary>
    /// Formats a number of bytes into a human readable string using decimal (1000 based)
    /// units with up to two decimals, e.g. 138,700,000 bytes becomes "138.7MB".
    /// </summary>
    /// <param name="bytes">Number of bytes.</param>
    /// <returns>Human readable representation using "kB", "MB", "GB" or "TB".</returns>
    public static string Format(long bytes)
    {
        string[] units = { "kB", "MB", "GB", "TB" };

        var value = bytes / 1000d;
        var unitIndex = 0;
        while (Math.Abs(value) >= 1000d && unitIndex < units.Length - 1)
        {
            value /= 1000d;
            unitIndex++;
        }

        return value.ToString("0.##", CultureInfo.InvariantCulture) + units[unitIndex];
    }

    /// <summary>
    /// Tries to parse a human readable memory quantity into a number of bytes.
    /// </summary>
    /// <param name="value">Memory quantity, e.g. "100MB" or "1GB".</param>
    /// <param name="bytes">The parsed number of bytes when successful; otherwise 0.</param>
    /// <returns><c>true</c> when the value was parsed successfully; otherwise <c>false</c>.</returns>
    public static bool TryParse(string? value, out long bytes)
    {
        bytes = 0;

        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var match = MemoryQuantityRegex().Match(value.Trim());
        if (!match.Success)
        {
            return false;
        }

        if (!decimal.TryParse(match.Groups["value"].Value, NumberStyles.Number, CultureInfo.InvariantCulture, out var number))
        {
            return false;
        }

        var binary = match.Groups["binary"].Success;
        var unit = match.Groups["unit"].Value;

        var @base = binary ? 1024m : 1000m;
        var exponent = match.Groups["prefix"].Value.ToUpperInvariant() switch
        {
            "K" => 1,
            "M" => 2,
            "G" => 3,
            "T" => 4,
            _ => 0
        };

        try
        {
            var multiplier = 1m;
            for (var i = 0; i < exponent; i++)
            {
                multiplier *= @base;
            }

            var total = number * multiplier;

            // Lowercase 'b' represents bits, otherwise the value is in bytes.
            if (unit == "b")
            {
                total /= 8m;
            }

            var rounded = Math.Round(total, MidpointRounding.AwayFromZero);
            if (rounded < 0m || rounded > long.MaxValue)
            {
                return false;
            }

            bytes = (long)rounded;
            return true;
        }
        catch (OverflowException)
        {
            return false;
        }
    }

    [GeneratedRegex(@"^(?<value>\d+(\.\d+)?)\s*(?<prefix>[kKmMgGtT])?(?<binary>i)?(?<unit>[bB])?$", RegexOptions.CultureInvariant)]
    private static partial Regex MemoryQuantityRegex();
}

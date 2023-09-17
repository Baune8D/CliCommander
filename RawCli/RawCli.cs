using CliWrap;

namespace RawCli;

/// <inheritdoc cref="Cli" />
public static class RawCli
{
    /// <inheritdoc cref="Cli.Wrap" />
    public static RawCommand Wrap(string targetFilePath) => new(targetFilePath);
}
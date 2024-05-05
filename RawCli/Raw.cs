namespace RawCli;

/// <inheritdoc cref="CliWrap.Cli" />
public static class Raw
{
    /// <inheritdoc cref="CliWrap.Cli.Wrap" />
    public static RawCommand CliWrap(string targetFilePath) => new(targetFilePath);
}

namespace RawCli;

/// <inheritdoc cref="CliWrap.Cli" />
public static class Raw
{
    /// <inheritdoc cref="CliWrap.Cli" />
    public static class Cli
    {
        /// <inheritdoc cref="CliWrap.Cli.Wrap" />
        public static RawCommand Wrap(string targetFilePath) => new(targetFilePath);
    }
}

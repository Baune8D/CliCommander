using CliWrap;

namespace CliCommander;

/// <inheritdoc cref="Cli" />
public class CliCommander
{
    /// <inheritdoc cref="Cli.Wrap" />
    public static Commander Wrap(string targetFilePath)
    {
        return new Commander(targetFilePath);
    }
}

using CliWrap;

namespace RawCli;

internal static class CommandResultValidationExtensions
{
    public static bool IsZeroExitCodeValidationEnabled(this CommandResultValidation validation) =>
        (validation & CommandResultValidation.ZeroExitCode) != 0;
}
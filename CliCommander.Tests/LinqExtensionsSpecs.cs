using CliWrap;
using FluentAssertions;
using RawCli;
using Xunit;

namespace CliCommander.Tests;

public class LinqExtensionsSpecs
{
    [Theory]
    [InlineData(true, CommandResultValidation.ZeroExitCode)]
    [InlineData(false, CommandResultValidation.None)]
    public void I_can_conditionally_convert_commander(bool shouldThrowOnExitCode, CommandResultValidation expectedValidation)
    {
        // Act
        var cmd = Commander.Wrap("foo")
            .When(!shouldThrowOnExitCode, c => c.WithValidation(CommandResultValidation.None));

        // Assert
        cmd.Validation.Should().Be(expectedValidation);
    }

    [Theory]
    [InlineData(true, CommandResultValidation.ZeroExitCode)]
    [InlineData(false, CommandResultValidation.None)]
    public void I_can_conditionally_convert_rawcommand(bool shouldThrowOnExitCode, CommandResultValidation expectedValidation)
    {
        // Act
        var cmd = Raw.CliWrap("foo")
            .When(!shouldThrowOnExitCode, c => c.WithValidation(CommandResultValidation.None));

        // Assert
        cmd.Validation.Should().Be(expectedValidation);
    }

    [Theory]
    [InlineData(true, CommandResultValidation.ZeroExitCode)]
    [InlineData(false, CommandResultValidation.None)]
    public void I_can_conditionally_convert_command(bool shouldThrowOnExitCode, CommandResultValidation expectedValidation)
    {
        // Act
        var cmd = Cli.Wrap("foo")
            .When(!shouldThrowOnExitCode, c => c.WithValidation(CommandResultValidation.None));

        // Assert
        cmd.Validation.Should().Be(expectedValidation);
    }
}

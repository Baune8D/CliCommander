using System.IO;
using CliWrap;
using FluentAssertions;
using RawCli;
using Xunit;

namespace CliCommander.Tests;

public class ConversionSpec
{
    [Fact(Timeout = 15000)]
    public void I_can_convert_a_command_with_the_default_configuration_to_cliwrap()
    {
        // Act
        var cmd = CliCommander.Wrap("foo").ToCliWrap();

        // Assert
        cmd.Should().BeOfType<Command>();
        cmd.TargetFilePath.Should().Be("foo");
        cmd.Arguments.Should().BeEmpty();
        cmd.WorkingDirPath.Should().Be(Directory.GetCurrentDirectory());
        cmd.Credentials.Should().BeEquivalentTo(Credentials.Default);
        cmd.EnvironmentVariables.Should().BeEmpty();
        cmd.Validation.Should().Be(CommandResultValidation.ZeroExitCode);
        cmd.StandardInputPipe.Should().Be(PipeSource.Null);
        cmd.StandardOutputPipe.Should().Be(PipeTarget.Null);
        cmd.StandardErrorPipe.Should().Be(PipeTarget.Null);
    }

    [Fact(Timeout = 15000)]
    public void I_can_convert_a_command_with_the_default_configuration_to_rawcli()
    {
        // Act
        var cmd = CliCommander.Wrap("foo").ToRawCli();

        // Assert
        cmd.Should().BeOfType<RawCommand>();
        cmd.TargetFilePath.Should().Be("foo");
        cmd.Arguments.Should().BeEmpty();
        cmd.WorkingDirPath.Should().Be(Directory.GetCurrentDirectory());
        cmd.Credentials.Should().BeEquivalentTo(Credentials.Default);
        cmd.EnvironmentVariables.Should().BeEmpty();
        cmd.Validation.Should().Be(CommandResultValidation.ZeroExitCode);
        cmd.StandardInputPipe.Should().Be(PipeSource.Null);
        cmd.RedirectStandardInput.Should().BeTrue();
        cmd.RedirectStandardOutput.Should().BeFalse();
        cmd.RedirectStandardError.Should().BeFalse();
    }
}
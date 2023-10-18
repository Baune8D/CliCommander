using System.Threading.Tasks;
using CliWrap;
using CliWrap.Exceptions;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace RawCli.Tests;

public class ValidationSpecs
{
    private readonly ITestOutputHelper _testOutput;

    public ValidationSpecs(ITestOutputHelper testOutput) => _testOutput = testOutput;

    [Fact(Timeout = 15000)]
    public async Task I_can_execute_a_command_and_get_an_error_if_it_returns_a_non_zero_exit_code()
    {
        // Arrange
        var cmd = RawCli.Wrap("dotnet")
            .WithArguments(a => a
                .Add(Dummy.Program.FilePath)
                .Add("exit")
                .Add("--code").Add(1)
            );

        // Act & assert
        var ex = await Assert.ThrowsAsync<CommandExecutionException>(async () =>
            await cmd.WithStandardOutputToNull().ExecuteAsync()
        );

        ex.ExitCode.Should().Be(1);
        ex.Command.Should().BeEquivalentTo(cmd, o => o
            .Excluding(c => c.RedirectStandardInput)
            .Excluding(c => c.RedirectStandardOutput)
            .Excluding(c => c.RedirectStandardError));

        _testOutput.WriteLine(ex.Message);
    }

    [Fact(Timeout = 15000)]
    public async Task I_can_execute_a_command_without_validating_the_exit_code()
    {
        // Arrange
        var cmd = RawCli.Wrap("dotnet")
            .WithArguments(a => a
                .Add(Dummy.Program.FilePath)
                .Add("exit")
                .Add("--code").Add(1)
            )
            .WithValidation(CommandResultValidation.None);

        // Act
        var result = await cmd.WithStandardOutputToNull().ExecuteAsync();

        // Assert
        result.ExitCode.Should().Be(1);
    }
}
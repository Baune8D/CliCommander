using System;
using System.Threading;
using System.Threading.Tasks;
using CliWrap;
using Xunit;

namespace RawCli.Tests;

public class PipingSpecs
{
    [Fact(Timeout = 15000)]
    public async Task I_can_execute_a_command_and_get_an_error_if_the_pipe_source_throws_an_exception()
    {
        // Arrange
        var cmd = PipeSource.FromFile("non-existing-file.txt") | RawCli.Wrap("dotnet")
            .WithArguments(a => a
                .Add(Dummy.Program.FilePath)
                .Add("echo stdin")
            );

        // Act & assert
        await Assert.ThrowsAnyAsync<Exception>(async () => await cmd.ExecuteAsync());
    }

    [Fact(Timeout = 15000)]
    public async Task I_can_execute_a_command_and_not_hang_if_the_process_expects_stdin_but_none_is_provided()
    {
        // Arrange
        var cmd = RawCli.Wrap("dotnet")
            .WithArguments(a => a
                .Add(Dummy.Program.FilePath)
                .Add("echo stdin")
            );

        // Act
        await cmd.ExecuteAsync();
    }

    [Fact(Timeout = 15000)]
    public async Task I_can_execute_a_command_and_not_hang_if_the_process_expects_stdin_but_empty_data_is_provided()
    {
        // Arrange
        var cmd = Array.Empty<byte>() | RawCli.Wrap("dotnet")
            .WithArguments(a => a
                .Add(Dummy.Program.FilePath)
                .Add("echo stdin")
            );

        // Act
        await cmd.ExecuteAsync();
    }

    [Fact(Timeout = 15000)]
    public async Task I_can_execute_a_command_and_not_hang_if_the_process_only_partially_consumes_stdin()
    {
        // https://github.com/Tyrrrz/CliWrap/issues/74

        // Arrange
        var random = new Random(1234567);

        var source = PipeSource.Create(async (destination, cancellationToken) =>
        {
            var buffer = new byte[256];
            while (true)
            {
                random.NextBytes(buffer);
                await destination.WriteAsync(buffer, cancellationToken);
            }

            // ReSharper disable once FunctionNeverReturns
        });

        var cmd = source | RawCli.Wrap("dotnet")
            .WithArguments(a => a
                .Add(Dummy.Program.FilePath)
                .Add("echo stdin")
                .Add("--length").Add(100_000)
            );

        // Act & assert
        await cmd.ExecuteAsync();
    }

    [Fact(Timeout = 15000)]
    public async Task I_can_execute_a_command_and_not_hang_if_the_process_does_not_consume_stdin()
    {
        // https://github.com/Tyrrrz/CliWrap/issues/74

        // Arrange
        var source = PipeSource.Create(async (_, cancellationToken) =>
        {
            // Not infinite, but long enough
            await Task.Delay(TimeSpan.FromSeconds(20), cancellationToken);
        });

        var cmd = source | RawCli.Wrap("dotnet")
            .WithArguments(a => a
                .Add(Dummy.Program.FilePath)
                .Add("echo stdin")
                .Add("--length").Add(0)
            );

        // Act & assert
        await cmd.ExecuteAsync();
    }

    [Fact(Timeout = 15000)]
    public async Task I_can_execute_a_command_and_not_hang_if_the_process_does_not_consume_stdin_even_if_the_source_cannot_be_canceled()
    {
        // https://github.com/Tyrrrz/CliWrap/issues/74

        // Arrange
        var source = PipeSource.Create(async (_, _) =>
            // Not infinite, but long enough
            await Task.Delay(TimeSpan.FromSeconds(20), CancellationToken.None)
        );

        var cmd = source | RawCli.Wrap("dotnet")
            .WithArguments(a => a
                .Add(Dummy.Program.FilePath)
                .Add("echo stdin")
                .Add("--length").Add(0)
            );

        // Act & assert
        await cmd.ExecuteAsync();
    }

    [Fact(Timeout = 15000)]
    public async Task I_can_execute_a_command_and_not_hang_on_large_stdin_while_also_writing_stdout()
    {
        // https://github.com/Tyrrrz/CliWrap/issues/61

        // Arrange
        var random = new Random(1234567);
        var bytesRemaining = 100_000;

        var source = PipeSource.Create(async (destination, cancellationToken) =>
        {
            var buffer = new byte[256];
            while (bytesRemaining > 0)
            {
                random.NextBytes(buffer);

                var count = Math.Min(bytesRemaining, buffer.Length);
                await destination.WriteAsync(buffer.AsMemory()[..count], cancellationToken);

                bytesRemaining -= count;
            }
        });

        var cmd = source | RawCli.Wrap("dotnet")
            .WithArguments(a => a
                .Add(Dummy.Program.FilePath)
                .Add("echo stdin")
            );

        // Act & assert
        await cmd.ExecuteAsync();
    }
}
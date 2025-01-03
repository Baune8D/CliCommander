using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.IO;
using CliWrap;

namespace RawCli;

/// <inheritdoc cref="Command" />
public partial class RawCommand : CommandBase<RawCommand>
{
    /// <summary>
    /// Standard input redirect value of the underlying process.
    /// </summary>
    public bool RedirectStandardInput { get; }

    /// <summary>
    /// Standard output redirect value of the underlying process.
    /// </summary>
    public bool RedirectStandardOutput { get; }

    /// <summary>
    /// Standard error redirect value of the underlying process.
    /// </summary>
    public bool RedirectStandardError { get; }

    /// <inheritdoc cref="Command.StandardOutputPipe" />
    private static PipeTarget StandardOutputPipe => PipeTarget.Null;

    /// <inheritdoc cref="Command.StandardErrorPipe" />
    private static PipeTarget StandardErrorPipe => PipeTarget.Null;

    /// <summary>
    /// Initializes an instance of <see cref="RawCommand" />.
    /// </summary>
    public RawCommand(
        string targetFilePath,
        string arguments,
        string workingDirPath,
        Credentials credentials,
        IReadOnlyDictionary<string, string?> environmentVariables,
        CommandResultValidation validation,
        PipeSource standardInputPipe
    )
        : base(
            targetFilePath,
            arguments,
            workingDirPath,
            credentials,
            environmentVariables,
            validation,
            standardInputPipe
        )
    {
        RedirectStandardInput = true;
        RedirectStandardOutput = false;
        RedirectStandardError = false;
    }

    /// <summary>
    /// Initializes an instance of <see cref="RawCommand" />.
    /// </summary>
    public RawCommand(
        string targetFilePath,
        string arguments,
        string workingDirPath,
        Credentials credentials,
        IReadOnlyDictionary<string, string?> environmentVariables,
        CommandResultValidation validation,
        PipeSource standardInputPipe,
        bool redirectStandardInput,
        bool redirectStandardOutput,
        bool redirectStandardError
    )
        : this(
            targetFilePath,
            arguments,
            workingDirPath,
            credentials,
            environmentVariables,
            validation,
            standardInputPipe
        )
    {
        RedirectStandardInput = redirectStandardInput;
        RedirectStandardOutput = redirectStandardOutput;
        RedirectStandardError = redirectStandardError;
    }

    /// <summary>
    /// Initializes an instance of <see cref="RawCommand" />.
    /// </summary>
    public RawCommand(string targetFilePath)
        : this(
            targetFilePath,
            string.Empty,
            Directory.GetCurrentDirectory(),
            Credentials.Default,
            new Dictionary<string, string?>(),
            CommandResultValidation.ZeroExitCode,
            PipeSource.Null,
            true,
            false,
            false
        ) { }

    /// <inheritdoc />
    [Pure]
    public override RawCommand WithTargetFile(string targetFilePath) =>
        new(
            targetFilePath,
            Arguments,
            WorkingDirPath,
            Credentials,
            EnvironmentVariables,
            Validation,
            StandardInputPipe,
            RedirectStandardInput,
            RedirectStandardOutput,
            RedirectStandardError
        );

    /// <inheritdoc />
    [Pure]
    public override RawCommand WithArguments(string arguments) =>
        new(
            TargetFilePath,
            arguments,
            WorkingDirPath,
            Credentials,
            EnvironmentVariables,
            Validation,
            StandardInputPipe,
            RedirectStandardInput,
            RedirectStandardOutput,
            RedirectStandardError
        );

    /// <inheritdoc />
    [Pure]
    public override RawCommand WithWorkingDirectory(string workingDirPath) =>
        new(
            TargetFilePath,
            Arguments,
            workingDirPath,
            Credentials,
            EnvironmentVariables,
            Validation,
            StandardInputPipe,
            RedirectStandardInput,
            RedirectStandardOutput,
            RedirectStandardError
        );

    /// <inheritdoc />
    [Pure]
    public override RawCommand WithCredentials(Credentials credentials) =>
        new(
            TargetFilePath,
            Arguments,
            WorkingDirPath,
            credentials,
            EnvironmentVariables,
            Validation,
            StandardInputPipe,
            RedirectStandardInput,
            RedirectStandardOutput,
            RedirectStandardError
        );

    /// <inheritdoc />
    [Pure]
    public override RawCommand WithEnvironmentVariables(
        IReadOnlyDictionary<string, string?> environmentVariables
    ) =>
        new(
            TargetFilePath,
            Arguments,
            WorkingDirPath,
            Credentials,
            environmentVariables,
            Validation,
            StandardInputPipe,
            RedirectStandardInput,
            RedirectStandardOutput,
            RedirectStandardError
        );

    /// <inheritdoc />
    [Pure]
    public override RawCommand WithValidation(CommandResultValidation validation) =>
        new(
            TargetFilePath,
            Arguments,
            WorkingDirPath,
            Credentials,
            EnvironmentVariables,
            validation,
            StandardInputPipe,
            RedirectStandardInput,
            RedirectStandardOutput,
            RedirectStandardError
        );

    /// <inheritdoc />
    [Pure]
    public override RawCommand WithStandardInputPipe(PipeSource source) =>
        new(
            TargetFilePath,
            Arguments,
            WorkingDirPath,
            Credentials,
            EnvironmentVariables,
            Validation,
            source,
            RedirectStandardInput,
            RedirectStandardOutput,
            RedirectStandardError
        );

    /// <summary>
    /// Creates a copy of this command, setting redirect of standard input.
    /// </summary>
    [Pure]
    public RawCommand WithStandardInputRedirect(bool redirect) =>
        new(
            TargetFilePath,
            Arguments,
            WorkingDirPath,
            Credentials,
            EnvironmentVariables,
            Validation,
            StandardInputPipe,
            redirect,
            RedirectStandardOutput,
            RedirectStandardError
        );

    /// <summary>
    /// Creates a copy of this command, setting redirect of standard output to null device.
    /// </summary>
    [Pure]
    public RawCommand WithStandardOutputToNull() =>
        new(
            TargetFilePath,
            Arguments,
            WorkingDirPath,
            Credentials,
            EnvironmentVariables,
            Validation,
            StandardInputPipe,
            RedirectStandardInput,
            true,
            RedirectStandardError
        );

    /// <summary>
    /// Creates a copy of this command, setting redirect of standard error to null device.
    /// </summary>
    [Pure]
    public RawCommand WithStandardErrorToNull() =>
        new(
            TargetFilePath,
            Arguments,
            WorkingDirPath,
            Credentials,
            EnvironmentVariables,
            Validation,
            StandardInputPipe,
            RedirectStandardInput,
            RedirectStandardOutput,
            true
        );

    /// <summary>
    /// Creates a copy of this command, setting redirect of all output to null device.
    /// </summary>
    [Pure]
    public RawCommand WithHiddenOutput() =>
        new(
            TargetFilePath,
            Arguments,
            WorkingDirPath,
            Credentials,
            EnvironmentVariables,
            Validation,
            StandardInputPipe,
            RedirectStandardInput,
            true,
            true
        );

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override string ToString() => AsString();
}
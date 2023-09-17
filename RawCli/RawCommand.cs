using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.IO;
using CliWrap;
using CliWrap.Builders;

namespace RawCli;

/// <inheritdoc cref="Command" />
public partial class RawCommand
{
    /// <inheritdoc cref="Command.TargetFilePath" />
    public string TargetFilePath { get; }

    /// <inheritdoc cref="Command.Arguments" />
    public string Arguments { get; }

    /// <inheritdoc cref="Command.WorkingDirPath" />
    public string WorkingDirPath { get; }

    /// <inheritdoc cref="Command.Credentials" />
    public Credentials Credentials { get; }

    /// <inheritdoc cref="Command.EnvironmentVariables" />
    public IReadOnlyDictionary<string, string?> EnvironmentVariables { get; }

    /// <inheritdoc cref="Command.Validation" />
    public CommandResultValidation Validation { get; }

    /// <inheritdoc cref="Command.StandardInputPipe" />
    public PipeSource StandardInputPipe { get; }
    
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
        PipeSource standardInputPipe,
        bool redirectStandardInput,
        bool redirectStandardOutput,
        bool redirectStandardError)
    {
        TargetFilePath = targetFilePath;
        Arguments = arguments;
        WorkingDirPath = workingDirPath;
        Credentials = credentials;
        EnvironmentVariables = environmentVariables;
        Validation = validation;
        StandardInputPipe = standardInputPipe;
        RedirectStandardInput = redirectStandardInput;
        RedirectStandardOutput = redirectStandardOutput;
        RedirectStandardError = redirectStandardError;
    }

    /// <summary>
    /// Initializes an instance of <see cref="RawCommand" />.
    /// </summary>
    public RawCommand(string targetFilePath) : this(
        targetFilePath,
        string.Empty,
        Directory.GetCurrentDirectory(),
        Credentials.Default,
        new Dictionary<string, string?>(),
        CommandResultValidation.ZeroExitCode,
        PipeSource.Null,
        true,
        false,
        false)
    {
    }

    /// <inheritdoc cref="Command.WithTargetFile(string)" />
    [Pure]
    public RawCommand WithTargetFile(string targetFilePath) => new(
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

    /// <inheritdoc cref="Command.WithArguments(string)" />
    [Pure]
    public RawCommand WithArguments(string arguments) => new(
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

    /// <inheritdoc cref="Command.WithArguments(IEnumerable&lt;string&gt;, bool)" />
    [Pure]
    public RawCommand WithArguments(IEnumerable<string> arguments, bool escape) =>
        WithArguments(args => args.Add(arguments, escape));

    /// <inheritdoc cref="Command.WithArguments(IEnumerable&lt;string&gt;)" />
    [Pure]
    public RawCommand WithArguments(IEnumerable<string> arguments) =>
        WithArguments(arguments, true);

    /// <inheritdoc cref="Command.WithArguments(Action&lt;ArgumentsBuilder&gt;)" />
    [Pure]
    public RawCommand WithArguments(Action<ArgumentsBuilder> configure)
    {
        var builder = new ArgumentsBuilder();
        configure(builder);

        return WithArguments(builder.Build());
    }

    /// <inheritdoc cref="Command.WithWorkingDirectory(string)" />
    [Pure]
    public RawCommand WithWorkingDirectory(string workingDirPath) => new(
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

    /// <inheritdoc cref="Command.WithCredentials(Credentials)" />
    [Pure]
    public RawCommand WithCredentials(Credentials credentials) => new(
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

    /// <inheritdoc cref="Command.WithCredentials(Action&lt;CredentialsBuilder&gt;)" />
    [Pure]
    public RawCommand WithCredentials(Action<CredentialsBuilder> configure)
    {
        var builder = new CredentialsBuilder();
        configure(builder);

        return WithCredentials(builder.Build());
    }

    /// <inheritdoc cref="Command.WithEnvironmentVariables(IReadOnlyDictionary&lt;string, string?&gt;)" />
    [Pure]
    public RawCommand WithEnvironmentVariables(IReadOnlyDictionary<string, string?> environmentVariables) => new(
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

    /// <inheritdoc cref="Command.WithEnvironmentVariables(Action&lt;EnvironmentVariablesBuilder?&gt;)" />
    [Pure]
    public RawCommand WithEnvironmentVariables(Action<EnvironmentVariablesBuilder> configure)
    {
        var builder = new EnvironmentVariablesBuilder();
        configure(builder);

        return WithEnvironmentVariables(builder.Build());
    }

    /// <inheritdoc cref="Command.WithValidation(CommandResultValidation)" />
    [Pure]
    public RawCommand WithValidation(CommandResultValidation validation) => new(
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

    /// <inheritdoc cref="Command.WithStandardInputPipe(PipeSource)" />
    [Pure]
    public RawCommand WithStandardInputPipe(PipeSource source) => new(
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
    public RawCommand WithStandardInputRedirect(bool redirect) => new(
        TargetFilePath,
        Arguments,
        WorkingDirPath,
        Credentials,
        EnvironmentVariables,
        Validation,
        StandardInputPipe,
        redirect,
        RedirectStandardOutput,
        RedirectStandardError);

    /// <summary>
    /// Creates a copy of this command, setting redirect of standard output to null device.
    /// </summary>
    public RawCommand WithStandardOutputToNull() => new(
        TargetFilePath,
        Arguments,
        WorkingDirPath,
        Credentials,
        EnvironmentVariables,
        Validation,
        StandardInputPipe,
        RedirectStandardInput,
        true,
        RedirectStandardError);
    
    /// <summary>
    /// Creates a copy of this command, setting redirect of standard error to null device.
    /// </summary>
    public RawCommand WithStandardErrorToNull() => new(
        TargetFilePath,
        Arguments,
        WorkingDirPath,
        Credentials,
        EnvironmentVariables,
        Validation,
        StandardInputPipe,
        RedirectStandardInput,
        RedirectStandardOutput,
        true);

    /// <summary>
    /// Creates a copy of this command, setting redirect of all output to null device.
    /// </summary>
    public RawCommand WithHiddenOutput() => new(
        TargetFilePath,
        Arguments,
        WorkingDirPath,
        Credentials,
        EnvironmentVariables,
        Validation,
        StandardInputPipe,
        RedirectStandardInput,
        true,
        true);
    
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override string ToString() => $"{TargetFilePath} {Arguments}";
}
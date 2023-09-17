using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using CliWrap;
using CliWrap.Builders;

namespace RawCli;

/// <summary>
/// Shared base command for aligning our own classes with CliWrap fluent API.
/// </summary>
public interface ICommandBase
{
    /// <inheritdoc cref="ICommandConfiguration.TargetFilePath" />
    string TargetFilePath { get; }

    /// <inheritdoc cref="ICommandConfiguration.Arguments" />
    string Arguments { get; }

    /// <inheritdoc cref="ICommandConfiguration.WorkingDirPath" />
    string WorkingDirPath { get; }

    /// <inheritdoc cref="ICommandConfiguration.Credentials" />
    Credentials Credentials { get; }

    /// <inheritdoc cref="ICommandConfiguration.EnvironmentVariables" />
    IReadOnlyDictionary<string, string?> EnvironmentVariables { get; }

    /// <inheritdoc cref="ICommandConfiguration.Validation" />
    CommandResultValidation Validation { get; }

    /// <inheritdoc cref="ICommandConfiguration.StandardInputPipe" />
    PipeSource StandardInputPipe { get; }
}

/// <summary>
/// Shared base command for aligning our own classes with CliWrap fluent API.
/// </summary>
public abstract class CommandBase<T> : ICommandBase
    where T : ICommandBase
{
    /// <summary>
    /// Initializes an instance of <see cref="CommandBase{T}" />.
    /// </summary>
    public CommandBase(
        string targetFilePath,
        string arguments,
        string workingDirPath,
        Credentials credentials,
        IReadOnlyDictionary<string, string?> environmentVariables,
        CommandResultValidation validation,
        PipeSource standardInputPipe
    )
    {
        TargetFilePath = targetFilePath;
        Arguments = arguments;
        WorkingDirPath = workingDirPath;
        Credentials = credentials;
        EnvironmentVariables = environmentVariables;
        Validation = validation;
        StandardInputPipe = standardInputPipe;
    }
    
    /// <summary>
    /// Initializes an instance of <see cref="CommandBase{T}" />.
    /// </summary>
    public CommandBase(string targetFilePath)
        : this(
            targetFilePath,
            string.Empty,
            Directory.GetCurrentDirectory(),
            Credentials.Default,
            new Dictionary<string, string?>(),
            CommandResultValidation.ZeroExitCode,
            PipeSource.Null
        ) { }

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

    /// <inheritdoc cref="Command.WithTargetFile(string)" />
    public abstract T WithTargetFile(string targetFilePath);
    
    /// <inheritdoc cref="Command.WithArguments(string)" />
    public abstract T WithArguments(string arguments);

    /// <inheritdoc cref="Command.WithArguments(IEnumerable&lt;string&gt;, bool)" />
    public T WithArguments(IEnumerable<string> arguments, bool escape) =>
        WithArguments(args => args.Add(arguments, escape));

    /// <inheritdoc cref="Command.WithArguments(IEnumerable&lt;string&gt;)" />
    public T WithArguments(IEnumerable<string> arguments) =>
        WithArguments(arguments, true);

    /// <inheritdoc cref="Command.WithArguments(Action&lt;ArgumentsBuilder&gt;)" />
    public T WithArguments(Action<ArgumentsBuilder> configure)
    {
        var builder = new ArgumentsBuilder();
        configure(builder);

        return WithArguments(builder.Build());
    }

    /// <inheritdoc cref="Command.WithWorkingDirectory(string)" />
    public abstract T WithWorkingDirectory(string workingDirPath);

    /// <inheritdoc cref="Command.WithCredentials(Credentials)" />
    public abstract T WithCredentials(Credentials credentials);

    /// <inheritdoc cref="Command.WithCredentials(Action&lt;CredentialsBuilder&gt;)" />
    public T WithCredentials(Action<CredentialsBuilder> configure)
    {
        var builder = new CredentialsBuilder();
        configure(builder);

        return WithCredentials(builder.Build());
    }
    
    /// <inheritdoc cref="Command.WithEnvironmentVariables(IReadOnlyDictionary&lt;string, string?&gt;)" />
    public abstract T WithEnvironmentVariables(IReadOnlyDictionary<string, string?> environmentVariables);

    /// <inheritdoc cref="Command.WithEnvironmentVariables(Action&lt;EnvironmentVariablesBuilder?&gt;)" />
    public T WithEnvironmentVariables(Action<EnvironmentVariablesBuilder> configure)
    {
        var builder = new EnvironmentVariablesBuilder();
        configure(builder);

        return WithEnvironmentVariables(builder.Build());
    }

    /// <inheritdoc cref="Command.WithValidation(CommandResultValidation)" />
    public abstract T WithValidation(CommandResultValidation validation);

    /// <inheritdoc cref="Command.WithStandardInputPipe(PipeSource)" />
    public abstract T WithStandardInputPipe(PipeSource source);

    /// <inheritdoc cref="Command.ToString()" />
    [ExcludeFromCodeCoverage]
    protected string AsString() => $"{TargetFilePath} {Arguments}";
}

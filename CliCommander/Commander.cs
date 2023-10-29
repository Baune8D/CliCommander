using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using CliWrap;
using RawCli;

namespace CliCommander;

/// <summary>
/// Has the same basic API as a CliWrap Command, but can also be converted to RawCli.
/// </summary>
public class Commander : CommandBase<Commander>
{
    /// <summary>
    /// Initializes an instance of <see cref="Commander" />.
    /// </summary>
    public Commander(
        string targetFilePath,
        string arguments,
        string workingDirPath,
        Credentials credentials,
        IReadOnlyDictionary<string, string?> environmentVariables,
        CommandResultValidation validation,
        PipeSource standardInputPipe)
        : base(
            targetFilePath,
            arguments,
            workingDirPath,
            credentials,
            environmentVariables,
            validation,
            standardInputPipe
        ) { }

    /// <summary>
    /// Initializes an instance of <see cref="CliCommander" />.
    /// </summary>
    public Commander(string targetFilePath)
        : base(targetFilePath)
    { }

    /// <inheritdoc cref="Cli.Wrap" />
    public static Commander Wrap(string targetFilePath)
    {
        return new Commander(targetFilePath);
    }
    
    /// <inheritdoc />
    [Pure]
    public override Commander WithTargetFile(string targetFilePath) => new(
        targetFilePath,
        Arguments,
        WorkingDirPath,
        Credentials,
        EnvironmentVariables,
        Validation,
        StandardInputPipe);

    /// <inheritdoc />
    [Pure]
    public override Commander WithArguments(string arguments) => new(
        TargetFilePath,
        arguments,
        WorkingDirPath,
        Credentials,
        EnvironmentVariables,
        Validation,
        StandardInputPipe);

    /// <inheritdoc />
    [Pure]
    public override Commander WithWorkingDirectory(string workingDirPath) => new(
        TargetFilePath,
        Arguments,
        workingDirPath,
        Credentials,
        EnvironmentVariables,
        Validation,
        StandardInputPipe);

    /// <inheritdoc />
    [Pure]
    public override Commander WithCredentials(Credentials credentials) => new(
        TargetFilePath,
        Arguments,
        WorkingDirPath,
        credentials,
        EnvironmentVariables,
        Validation,
        StandardInputPipe);

    /// <inheritdoc />
    [Pure]
    public override Commander WithEnvironmentVariables(IReadOnlyDictionary<string, string?> environmentVariables) => new(
        TargetFilePath,
        Arguments,
        WorkingDirPath,
        Credentials,
        environmentVariables,
        Validation,
        StandardInputPipe);

    /// <inheritdoc />
    [Pure]
    public override Commander WithValidation(CommandResultValidation validation) => new(
        TargetFilePath,
        Arguments,
        WorkingDirPath,
        Credentials,
        EnvironmentVariables,
        validation,
        StandardInputPipe);

    /// <inheritdoc />
    [Pure]
    public override Commander WithStandardInputPipe(PipeSource source) => new(
        TargetFilePath,
        Arguments,
        WorkingDirPath,
        Credentials,
        EnvironmentVariables,
        Validation,
        source);

    /// <summary>
    /// Converts this Commander instance to a RawCli command.
    /// </summary>
    [Pure]
    public RawCommand ToRawCli(bool hideOutput = false)
    {
        return new RawCommand(
            TargetFilePath,
            Arguments,
            WorkingDirPath,
            Credentials,
            EnvironmentVariables,
            Validation,
            StandardInputPipe,
            true,
            hideOutput,
            hideOutput);
    }

    /// <summary>
    /// Converts this Commander instance to a CliWrap command.
    /// </summary>
    [Pure]
    public Command ToCliWrap()
    {
        return new Command(
            TargetFilePath,
            Arguments,
            WorkingDirPath,
            Credentials,
            EnvironmentVariables,
            Validation,
            StandardInputPipe,
            PipeTarget.Null,
            PipeTarget.Null);
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override string ToString() => AsString();
}

using System;
using System.Diagnostics.Contracts;
using System.IO;
using CliWrap;

namespace RawCli;

public partial class RawCommand
{
    /// <summary>
    /// Creates a new command that pipes its standard input from the specified source.
    /// </summary>
    [Pure]
    public static RawCommand operator |(PipeSource source, RawCommand target) =>
        target.WithStandardInputPipe(source);

    /// <summary>
    /// Creates a new command that pipes its standard input from the specified stream.
    /// </summary>
    [Pure]
    public static RawCommand operator |(Stream source, RawCommand target) =>
        PipeSource.FromStream(source) | target;

    /// <summary>
    /// Creates a new command that pipes its standard input from the specified memory buffer.
    /// </summary>
    [Pure]
    public static RawCommand operator |(ReadOnlyMemory<byte> source, RawCommand target) =>
        PipeSource.FromBytes(source) | target;

    /// <summary>
    /// Creates a new command that pipes its standard input from the specified byte array.
    /// </summary>
    [Pure]
    public static RawCommand operator |(byte[] source, RawCommand target) =>
        PipeSource.FromBytes(source) | target;

    /// <summary>
    /// Creates a new command that pipes its standard input from the specified string.
    /// Uses <see cref="Console.InputEncoding" /> for encoding.
    /// </summary>
    [Pure]
    public static RawCommand operator |(string source, RawCommand target) =>
        PipeSource.FromString(source) | target;

    /// <summary>
    /// Creates a new command that pipes its standard input from the standard output of the
    /// specified command.
    /// </summary>
    [Pure]
    public static RawCommand operator |(Command source, RawCommand target) =>
        PipeSource.FromCommand(source) | target;
}

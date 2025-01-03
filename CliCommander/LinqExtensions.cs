using System;
using CliWrap;
using RawCli;

namespace CliCommander;

/// <summary>
/// Helper methods for conditional command modification.
/// </summary>
public static class LinqExtensions
{
    /// <summary>
    /// Conditional modification of command object.
    /// </summary>
    public static Commander When(
        this Commander obj,
        bool condition,
        Func<Commander, Commander> action
    )
    {
        return condition ? action.Invoke(obj) : obj;
    }

    /// <summary>
    /// Conditional modification of command object.
    /// </summary>
    public static RawCommand When(
        this RawCommand obj,
        bool condition,
        Func<RawCommand, RawCommand> action
    )
    {
        return condition ? action.Invoke(obj) : obj;
    }

    /// <summary>
    /// Conditional modification of command object.
    /// </summary>
    public static Command When(this Command obj, bool condition, Func<Command, Command> action)
    {
        return condition ? action.Invoke(obj) : obj;
    }
}

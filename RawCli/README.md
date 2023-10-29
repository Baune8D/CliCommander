# RawCli
A fork of [CliWrap](https://github.com/Tyrrrz/CliWrap) that enables outputting native process output at the cost of output redirection.

```csharp
CommandResult result = Raw.CliWrap("docker")
    .WithArguments(a => a
        .Add("build")
        .Add("--progress")
        .Add("tty"))
    .Execute();
```

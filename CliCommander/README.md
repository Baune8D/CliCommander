# CliCommander
A shared API between `CliWrap` and `RawCli` that enables the creation of commands that can be executed by either library.

```csharp
var command = Commander.Wrap("docker")
    .WithArguments(a => a
        .Add("build")
        .Add("--progress")
        .Add("auto"));

// Docker will build with progress=plain since TTY is not supported when output is redirected.
BufferedCommandResult bufferedResult = command.ToCliWrap()
    .ExecuteBuffered();

// Docker will build with progress=tty since the output is not redirected.
CommandResult result = command.ToRawCli()
    .Execute();
```

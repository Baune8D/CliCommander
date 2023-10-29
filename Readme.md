# CliCommander
[![Build status](https://ci.appveyor.com/api/projects/status/409xh3bggth075qc?svg=true)](https://ci.appveyor.com/project/Baune8D/CliCommander)
[![codecov](https://codecov.io/gh/Baune8D/CliCommander/branch/master/graph/badge.svg)](https://codecov.io/gh/Baune8D/CliCommander)
[![NuGet Badge](https://buildstats.info/nuget/CliCommander)](https://www.nuget.org/packages/CliCommander)

A shared API between [CliWrap](https://github.com/Tyrrrz/CliWrap) and [RawCli](#RawCli) that enables the creation of commands that can be executed by either library.

### Usage:

```csharp
var command = CliCommander.Wrap("docker")
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

# RawCli
[![NuGet Badge](https://buildstats.info/nuget/RawCli)](https://www.nuget.org/packages/RawCli)

A fork of [CliWrap](https://github.com/Tyrrrz/CliWrap) that enables outputting native process output at the cost of output redirection.

The point of `Rawcli` is to support the following use case: https://github.com/Tyrrrz/CliWrap/issues/199 without having to use the raw `Process` object.

### Usage:

```csharp
CommandResult result = Raw.CliWrap("docker")
    .WithArguments(a => a
        .Add("build")
        .Add("--progress")
        .Add("tty"))
    .Execute();
```

Standard input redirection is still enabled by default. It can be disabled with: `.WithStandardInputRedirect(false)`

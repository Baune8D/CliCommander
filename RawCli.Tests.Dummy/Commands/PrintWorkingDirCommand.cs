using System.IO;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

namespace RawCli.Tests.Dummy.Commands;

[Command("print cwd")]
public class PrintWorkingDirCommand : ICommand
{
    public async ValueTask ExecuteAsync(IConsole console)
    {
        await console.Output.WriteAsync(Directory.GetCurrentDirectory());
    }
}
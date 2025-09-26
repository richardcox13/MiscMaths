module HashadPart2.Program

open System
open System.Diagnostics
open System.Threading.Tasks
open HashadPart2

let asyncMain (argv: string array): Task<int> =
    try
        printfn $"Hello from F# on {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}"
        let rootcmd = Cli.buildCli()
        let cliParse = rootcmd.Parse(argv)
        task {
            cliParse.InvocationConfiguration.EnableDefaultExceptionHandler <- false
            return! cliParse.InvokeAsync()
        }
    with ex ->
        let err = Console.Error
        err.WriteLine($"Unhandled Exception {ex.GetType().Name}: {ex.Message}")
        err.WriteLine(ex.StackTrace)
        if Debugger.IsAttached then Debugger.Break()
        Task.FromResult 1

[<EntryPoint>]
let main argv =
  task {
    do! Async.SwitchToThreadPool ()
    return! asyncMain argv
  } |> Async.AwaitTask |> Async.RunSynchronously

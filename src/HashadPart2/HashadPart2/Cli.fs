module HashadPart2.Cli

open System.CommandLine


let checkCommand() =
    let checkCommand = Command("check", "Check one or more ranges of numbers are Hashad numbers in given bases")
    checkCommand

let scanCommand() =
    let scanCommand = Command("scan", "Scan for Hashad numbers in given bases")
    scanCommand

let buildCli () =
    let rootCommand
         = RootCommand("Hashad number checker (part 2): check bases")
        |> fun cmd ->
            cmd.Add(checkCommand())
            cmd.Add(scanCommand())
            cmd
    rootCommand

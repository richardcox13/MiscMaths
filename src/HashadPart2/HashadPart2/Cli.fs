module HashadPart2.Cli

open System.CommandLine
open HashadPart2.NumberRange

let checkRangeArgument =
    Argument<NumberRange[]>("checkRange")
        |> fun arg -> (
            arg.Description <- "One or more numbers or ranges of numbers to check (ranges of form \"<n>..<m>\" (inclusive))"
            arg.Arity <- ArgumentArity.OneOrMore
            arg.DefaultValueFactory <- null
            arg.CustomParser <- (fun argRes -> 
                let parsed
                     = argRes.Tokens
                    |> Seq.map (fun t -> NumberRange.Parse t.Value)
                    |> Seq.toArray
                if Seq.exists (fun (r: Result<NumberRange,string>) -> r.IsError) parsed then
                    for err in (parsed |> Seq.choose (function | Result.Error e -> Some e | _ -> None)) do
                        argRes.AddError(err)
                    null
                else
                    parsed
                        |> Seq.map (function | Result.Ok v -> v | _ -> failwith "unreachable")
                        |> Seq.toArray
            )
            arg.Validators.Add(fun argRes ->
                let ranges = argRes.GetValueOrDefault<NumberRange[]>()
                if ranges <> null then
                    for r in ranges do
                        match r with
                        | Range (start, end_) when end_ < start ->
                            argRes.AddError($"Invalid range {r}: end must be greater than or equal to start")
                        | _ -> ()
            )
            arg
        )

let checkCommand =
    Command("check", "Check one or more ranges of numbers are Hashad numbers in given bases")
        |> fun cmd ->(
                cmd.Add(checkRangeArgument) 
                cmd.SetAction(fun ctx ->
                    let ranges = ctx.GetValue(checkRangeArgument)
                    printfn "Check command invoked for %s" (ranges |> Seq.map (fun s -> s.ToString()) |> String.concat ", ")
                    0)
                cmd)

let scanCommand =
    Command("scan", "Scan for Hashad numbers in given bases")
        |> fun cmd ->
                cmd.SetAction(fun ctx ->
                    printfn "Scan command invoked"
                    0)
                cmd

let buildCli () =
    let rootCommand
         = RootCommand("Hashad number checker (part 2): check bases")
        |> fun cmd ->
            cmd.Add(checkCommand)
            cmd.Add(scanCommand)
            cmd
    rootCommand

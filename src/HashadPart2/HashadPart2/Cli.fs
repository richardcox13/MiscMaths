module HashadPart2.Cli

open System.CommandLine
open HashadPart2.NumberRange
open System.CommandLine.Parsing

let parseNumberRangeArray (argRes: ArgumentResult) : NumberRange[] =
    // Do this in stages so errors reported eagerly.
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

let validateNumberRangeArray (ranges: NumberRange array) : string array =
    let errors = ResizeArray<string>()
    if ranges <> null then
        for r in ranges do
            match r with
            | Range (start, end_) when start < 2I ->
                errors.Add($"Invalid range {r}: start must be greater than 1")
            | Range (start, end_) when end_ < start ->
                errors.Add($"Invalid range {r}: end must be greater than or equal to start")
            | Single n when n < 2I ->
                errors.Add($"Invalid singleton {r}: number must be greater than 1")
            | _ -> ()
    errors.ToArray()

let makeBasesOption required =
    Option<NumberRange[]>("--bases")
        |> fun opt -> (
            opt.Description <- "One or more bases or ranges of bases (ranges of form \"<n>..<m>\" (inclusive))"
            opt.Aliases.Add("-b")
            opt.Arity <- if required then ArgumentArity.OneOrMore else ArgumentArity.ZeroOrMore
            opt.CustomParser <- parseNumberRangeArray
            opt.Validators.Add(fun optRes ->
                let ranges = optRes.GetValueOrDefault<NumberRange[]>()
                for e in validateNumberRangeArray ranges do
                    optRes.AddError(e)
            )
            opt)

let checkCommandBaesOption = makeBasesOption false
let scanCommandBasesOption = makeBasesOption true

let checkRangeArgument =
    Argument<NumberRange[]>("checkRange")
        |> fun arg -> (
            arg.Description <- "One or more numbers or ranges of numbers to check (ranges of form \"<n>..<m>\" (inclusive))"
            arg.Arity <- ArgumentArity.OneOrMore
            arg.DefaultValueFactory <- null
            arg.CustomParser <- parseNumberRangeArray
            arg.Validators.Add(fun argRes ->
                let ranges = argRes.GetValueOrDefault<NumberRange[]>()
                for e in validateNumberRangeArray ranges do
                    argRes.AddError(e)
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

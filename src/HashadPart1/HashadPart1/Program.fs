
open System
open System.Text.RegularExpressions

module HashadPart1 =

    let sumdigits (n: bigint) =
        let rec loop (n: bigint) (acc: bigint) : bigint =
            if n = 0I then acc
            else loop (n / 10I) (acc + (n % 10I))
        loop n 0I

    let test n =
        n % (sumdigits n) = 0I

    let numberOrRangeMatcher = Regex(@"^(\d+)(?:\.\.(\d+))?$")

    let showHelp () =
        printfn "Usage: HashadPart1 [<n> ...]"
        printfn "Without arguments prints all Hashad numbers (base 10) starting from 1."
        printfn "And will run indefinitely until interrupted."
        printfn "With arguments, tests each argument and prints whether it is a Hashad number."
        printfn "If an argument is of the form <n>..<m> will check the inclusive range."
        printfn "Or with \"-h\" shows this help."

    let checkSeqForHashads numbers =
        let mutable checks = 0
        let mutable hashads = 0
        for n in numbers do
            checks <- checks + 1
            let isHashad = test n
            if isHashad then
                printfn $"{n} is a Hashad number."
                hashads <- hashads + 1
            else
                printfn $"{n} is not a Hashad number."
        (checks, hashads)

    let seqFromArgs (ranges: (bigint * bigint) seq) =
        ranges
        |> Seq.collect (fun (start, finish) ->
            seq { for i in start .. finish -> i }              
        )

    [<EntryPoint>]
    let main args =

        if args.Length = 0 then
            let mutable n = 1I
            let mutable areHashad = 0
            let mutable stop = false
            Console.CancelKeyPress.Add(fun ea ->
                // Using printfn can be split across lines (as main loop interrupts it)
                Console.WriteLine($"Interrupe! ({ea.SpecialKey})")
                stop <- true
                ea.Cancel <- true
            )
            printfn "Hashad numbers (base 10):"
            while not stop do
                if test n then
                    printfn $"{n}"
                    areHashad <- areHashad + 1
                n <- n + 1I

            printfn $"Of {n} numbers tested, {areHashad} are Hashad numbers."
            0
        else if args[0].StartsWith("-h", StringComparison.CurrentCultureIgnoreCase)
                || args[0] = "-?"
                || Char.ToLower(args[0][0]) = 'h' then
            showHelp ()
            1
        else
            let toTest
                 = args
                |> Seq.map (fun a -> a, numberOrRangeMatcher.Match(a))

            if toTest |> Seq.exists (fun (a, m) -> not m.Success) then
                eprintfn "Can only test positive numbers, cannot process %A" (fst (toTest |> Seq.filter (fun (_, m) -> not m.Success) |> Seq.head))
                1
            else
                // Check ranges are valid.... need to parse into (start, end) tuples (bigint * bigint) first
                let ranges
                     = toTest
                    |> Seq.map snd
                    |> Seq.map (fun m ->
                        let start = bigint.Parse(m.Groups[1].Value)
                        start, if m.Groups[2].Success then bigint.Parse(m.Groups[2].Value) else start
                     )
                    |> Seq.filter (fun (start, finish) ->
                        if finish < start then
                            eprintfn "Range %A..%A is invalid, second number must be greater than or equal to the first, and both most be positive." start finish
                            false
                        else
                            true
                     )
                    |> Seq.toArray

                let (numberCount, countOfHashads) = ranges |> seqFromArgs |> checkSeqForHashads
                printfn ""
                printfn $"Of {numberCount} numbers tested, {countOfHashads} are Hashad numbers ({100.0 * float countOfHashads / float numberCount:F2}%%)."

                0

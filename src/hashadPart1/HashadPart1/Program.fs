
open System

module HashadPart1 =

    let sumdigits (n: bigint) =
        let rec loop (n: bigint) (acc: bigint) : bigint =
            if n = 0I then acc
            else loop (n / 10I) (acc + (n % 10I))
        loop n 0I

    let test n =
        n % (sumdigits n) = 0I

    [<EntryPoint>]
    let main args =

        if args.Length = 0 then
            printfn "Usage: HashadPart1"
            printfn "Prints all Hashad numbers (base 10) starting from 1."
            printfn "A Hashad number is an integer that is divisible by the sum of its digits."
            printfn "Example: 18 is a Hashad number because 1 + 8 = 9 and 18 is divisible by 9."
            printfn "This program will run indefinitely until interrupted."
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

            printfn $"Of {n} numbers tests, {areHashad} are Hashad numbers."
            0
        else if args[0].StartsWith("-h", StringComparison.CurrentCultureIgnoreCase)
                || args[0] = "-?"
                || Char.ToLower(args[0][0]) = 'h' then
            printfn "Usage: HashadPart1 [<n> ...]"
            printfn "Without arguments prints all Hashad numbers (base 10) starting from 1."
            printfn "And will run indefinitely until interrupted."
            printfn "With arguments, tests each argument and prints whether it is a Hashad number."
            printfn "Or with \"-h\" shows this help."
            1
        else
            let toTest
                 = args
                |> Seq.map bigint.TryParse

            if toTest |> Seq.exists (fun (p, _) -> not p) then
                eprintfn "Can only test numbers, cannot parse %A" (snd (toTest |> Seq.filter (fun (p, _) -> not p) |> Seq.head))
                1
            else
                for (_, v) in toTest do
                    let isHashad = test v
                    if isHashad then
                        printfn $"{v} is a Hashad number."
                    else
                        printfn $"{v} is not a Hashad number."
                0
        
        

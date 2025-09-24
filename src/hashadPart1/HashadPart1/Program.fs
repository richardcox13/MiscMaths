
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
            printfn "Hashad numbers (base 10):"
            while true do
                if test n then
                    printfn $"{n}"
                n <- n + 1I

            0
        else if args[0] = "-?" || Char.ToLower(args[0][0]) = 'h' then
            printfn "Usage: HashadPart1 [<n> ...]"
            printfn "Without arguments prints all Hashad numbers (base 10) starting from 1."
            printfn "And will run indefinitely until interrupted."
            printfn "With arguments, tests each argument and prints whether it is a Hashad number."
            printfn "Or with \"-h\" shows this help."
            0
        else
            printfn $"Invalid argument: {args[0]}"
            printfn "Use -? or h for help."
            1
        
        

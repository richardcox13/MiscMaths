module HashadPart2.Calculate

open System
open HashadPart2.NumberRange

let isHashadInBase (n: bigint) (base_: bigint) : bool =
    let rec sumOfDigits acc n =
        if n = 0I then acc
        else
            let digit = n % base_
            sumOfDigits (acc + digit) (n / base_)
    let s = sumOfDigits 0I n
    n % s = 0I

let expandNumberRanges (ranges: NumberRange seq): bigint seq =
    seq {
        for r in ranges do
            match r with
            | Single n -> yield seq { yield n}
            | Range (start, end_) ->
                yield seq {
                    for x = start to end_ do
                        yield x
                }
    }
    |> Seq.concat

let expandBases (bases: NumberRange seq) =
    expandNumberRanges bases
    |> Seq.distinct
    |> Seq.sort
    |> Seq.toArray

let defaultBases (toCheck: bigint array) =
    let max = toCheck |> Array.max
    let min = toCheck |> Array.min
    (min + 1I, max * 2I)

let scanNumberSequence (numbers: bigint seq) (bases: bigint array) =
    assert(bases.Length > 0)
    let mutable count = 0
    let mutable are = 0

    let mutable stop = false
    Console.CancelKeyPress.Add(fun ea ->
        // Using printfn can be split across lines (as main loop interrupts it)
        Console.WriteLine($"Interrupe! ({ea.SpecialKey})")
        stop <- true
        ea.Cancel <- true
    )

    for n in numbers |> Seq.takeWhile (fun _ -> not stop) do
        count <- count + 1
        Console.Write($"{n}: ")
        let hashadInBases =
            bases
            |> Seq.filter (fun b -> isHashadInBase n b)
            |> Seq.toArray
        if hashadInBases.Length = 0 then
            Console.WriteLine("Not a Harshad number in any base")
        else
            are <- are + 1
            let ss = hashadInBases |> Seq.map string |> String.concat ", "
            Console.WriteLine($"Harshad in bases {ss}")

    Console.WriteLine()
    Console.WriteLine($"Checked {count} numbers, {are} are Harshad in at least one base ({100.0 * float are / float count:F2}%%)")


let checkNumberRanges (toCheck: NumberRange array) (bases: NumberRange array) =
    let toCheck = expandNumberRanges toCheck |> Seq.toArray
    let bases =
        if bases.Length = 0 then
            let (minBase, maxBase) = defaultBases toCheck
            [| Range (minBase, maxBase) |]
        else
            bases
    let bases = expandBases bases
    let ss = bases |> Seq.map string |> String.concat ", "
    Console.WriteLine($"Checking in bases {ss}")

    scanNumberSequence toCheck bases

let scanNumbers (bases: NumberRange array) =
    let gen () =
        seq {
            let mutable n = 2I
            while true do
                yield n
                n <- n + 1I
        }
    scanNumberSequence (gen()) (expandBases bases)

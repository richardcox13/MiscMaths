module HashadPart2.NumberRange

open System.Text.RegularExpressions

let numberOrRangeMatcher = Regex(@"^(\d+)(?:\.\.(\d+))?$")

type NumberRange =
    | Single of bigint
    | Range of bigint * bigint
    override this.ToString() =
        match this with
        | Single n -> n.ToString()
        | Range (start, end_) -> $"{start}..{end_}"

    static member public Parse (s: string) : Result<NumberRange, string> =
        let m = numberOrRangeMatcher.Match(s)
        if not m.Success then
            Error $"Cannot parse \"{s}\" as a number range"
        else
            let start =  bigint.Parse(m.Groups.[1].Value)
            if m.Groups[2].Success then
                let end_ = bigint.Parse(m.Groups.[2].Value)
                Ok (Range (start, end_))
            else
                Ok (Single start)

# MiscMaths: Miscellaneous math-ish programs

# A. Hashad Numbers

Finding Hashad numbers (aka Niven numbers) in F#.

## Part A.1: Simple Base 10

If $S(n)$ is the sum of the base digits of $n$, then $n$ is Hashad if $S(n)$ divides $n$.

#### CLI Usage

If passed no arguments, just check numbers sequentially from 1 until interrupted.

If passed "-h" (or similar) show help.

If passed one or more numbers or ranges (`<n>..<m>`), check those numbers.

## Part A.2: Across Bases

When numbers are expressed in different bases $S(n)$ changes. Eg. $S_{10}(12) = 3$ in base 10, but in base 16 $S_{16}(12_{10}) = 12_{10}$ as $12_{10} = C_{16}$ (or `0x0C` in many progranmming languages).

### CLI

    HashadPart2 scan --bases <n>..<m> [<n>..<m>] ...

Scan from 1 upwards, checking each number of all the specified bases. At least one base must be specified.

    HashadPart2 check <n>..<m> [<n>..<m>] ... --bases <n>..<m> [<n>..<m>] ...

Check the specified numbers in the specified bases. If no bases are specified then in bases $[(n+1), 2m]$ where $n$ is the smallest number to check, and $m$ is the largest.

    HashadPart2 -h|--help|-?

Show help.

In all cases bases must be an integer greater than one.

## References

- https://en.wikipedia.org/wiki/Harshad_number
- [Numberphile on Hashad numbers](https://www.youtube.com/watch?v=dgwevhEykWQ)

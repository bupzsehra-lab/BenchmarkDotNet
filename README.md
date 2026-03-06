# BranchmarkStrings

Microbenchmarks for common string operations in C#, using [BenchmarkDotNet](https://benchmarkdotnet.org/).

---

## Benchmarks

### 1. String Comparison (`StringComparisonBenchmarks`)

Compares three ways to do a case-insensitive string equality check:

| Method | Approach |
|---|---|
| `Equals_OrdinalIgnoreCase` | `string.Equals(a, b, StringComparison.OrdinalIgnoreCase)` *(baseline)* |
| `ToLower` | `a.ToLower() == b` |
| `ToUpper` | `a.ToUpper() == b.ToUpper()` |

**Key takeaways:**
- `string.Equals` with `OrdinalIgnoreCase` is the fastest and allocates nothing
- `ToLower` / `ToUpper` allocate a new string on every call (~48 B) and are 2–3× slower

**Sample output:**

```
| Method                   | Mean     | Error | StdDev | Ratio    | Rank | Allocated |
|------------------------- |---------:|------:|-------:|---------:|-----:|----------:|
| Equals_OrdinalIgnoreCase | ~20 ns   | ...   | ...    | baseline |    1 |         - |
| ToLower                  | ~46 ns   | ...   | ...    |    +127% |    2 |      48 B |
| ToUpper                  | ~58 ns   | ...   | ...    |    +185% |    3 |      48 B |
```

---

### 2. String Concatenation (`StringConcatenationBenchmarks`)

Compares three ways to concatenate strings in a loop, parameterised over **10, 100, and 1000 iterations** to show how performance and allocation diverge as the loop count grows:

| Method | Approach |
|---|---|
| `PlusOperator` | `result += word` in a loop *(baseline)* |
| `StringBuilder` | `new StringBuilder(capacity)` + `Append` in a loop |
| `StringConcat` | Pre-build a `string[]`, then call `string.Concat(parts)` |

**Key takeaways:**
- `+` in a loop is O(n²) in allocations — each iteration copies the entire accumulated string
- `StringBuilder` with a pre-set capacity is the most efficient: near-zero extra allocations
- `string.Concat(string[])` is a good middle ground — one allocation pass, no intermediate strings

**Sample output (Iterations = 1000):**

```
| Method        | Iterations | Mean        | Ratio    | Rank | Allocated  |
|-------------- |----------- |------------:|---------:|-----:|-----------:|
| StringBuilder | 1000       |    ~5 μs    | baseline |    1 |    5.86 KB |
| StringConcat  | 1000       |    ~6 μs    |     +20% |    2 |   10.73 KB |
| PlusOperator  | 1000       | ~1,200 μs   |  +23900% |    3 |    3.81 MB |
```

---

## Metrics collected

All benchmarks report:
- **Mean** execution time
- **Allocation** (via `[MemoryDiagnoser]`)
- **Rank** across methods (via `[RankColumn]`)
- **Ratio** relative to the baseline (percentage style)

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

## Run

```bash
dotnet run -c Release
```

> BenchmarkDotNet requires **Release** mode. Debug builds will be rejected at startup.

To switch which benchmark runs, edit the `BenchmarkRunner.Run<>` call in `Program.cs`.

Results are written to `BenchmarkDotNet.Artifacts/results/` as `.html`, `.csv`, and `.md` files.

## Project structure

```
BranchmarkStrings/
├── StringEqualsBenchmark.cs          # String comparison benchmark
├── StringConcatenationBenchmarks.cs  # String concatenation benchmark
├── Program.cs                        # Entry point (BenchmarkRunner.Run)
├── BranchmarkStrings.csproj          # Project file (targets net10.0)
└── BranchmarkStrings.sln
```

# BranchmarkStrings

Microbenchmarks for common string comparison approaches in C#, using [BenchmarkDotNet](https://benchmarkdotnet.org/).

## What it benchmarks

Compares three ways to do a case-insensitive string equality check:

| Method | Approach |
|---|---|
| `Equals_OrdinalIgnoreCase` | `string.Equals(a, b, StringComparison.OrdinalIgnoreCase)` *(baseline)* |
| `ToLower` | `a.ToLower() == b` |
| `ToUpper` | `a.ToUpper() == b.ToUpper()` |

Metrics collected per run:
- **Mean / Median** execution time
- **Allocation** (via `[MemoryDiagnoser]`)
- **Rank** across methods (via `[RankColumn]`)
- **Ratio** relative to the baseline

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

## Run

```bash
dotnet run -c Release
```

> BenchmarkDotNet requires **Release** mode. Debug builds will be rejected at startup.

Results are written to `BenchmarkDotNet.Artifacts/results/` as `.html`, `.csv`, and `.md` files.

## Sample output

```
| Method                   | Mean     | Error    | StdDev   | Ratio    | Rank | Allocated |
|------------------------- |---------:|---------:|---------:|---------:|-----:|----------:|
| Equals_OrdinalIgnoreCase | ~20 ns   | ...      | ...      | baseline |    1 |         - |
| ToLower                  | ~46 ns   | ...      | ...      |    +127% |    2 |      48 B |
| ToUpper                  | ~58 ns   | ...      | ...      |    +185% |    3 |      48 B |
```

**Key takeaways:**
- `string.Equals` with `OrdinalIgnoreCase` is the fastest and allocates nothing
- `ToLower` / `ToUpper` allocate a new string on every call (~48 B) and are 2–3× slower

## Project structure

```
BranchmarkStrings/
├── StringEqualsBenchmark.cs   # Benchmark class
├── Program.cs                 # Entry point (BenchmarkRunner.Run)
├── BranchmarkStrings.csproj   # Project file (targets net10.0)
└── BranchmarkStrings.sln
```

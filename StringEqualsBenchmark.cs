using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;

namespace BenchmarkDotNet.Samples
{
    [MemoryDiagnoser]
    [RankColumn]
    [Config(typeof(Config))]
    [SimpleJob(RuntimeMoniker.Net10_0)]
    [HideColumns(Column.Job, Column.RatioSD, Column.AllocRatio)]
    public class StringComparisonBenchmarks
    {
        private class Config : ManualConfig
        {
            public Config()
            {
                SummaryStyle =
                    SummaryStyle.Default.WithRatioStyle(RatioStyle.Percentage);
            }
        }

    private readonly string _role = "ADMIN";
    private readonly string _target = "admin";

    [Benchmark(Baseline = true)]
    public bool Equals_OrdinalIgnoreCase() => string.Equals(_role, _target, StringComparison.OrdinalIgnoreCase);

    [Benchmark]
    public bool ToLower() => _role.ToLower() == _target;

    [Benchmark]
    public bool ToUpper() => _role.ToUpper() == _target.ToUpper();

    }
}
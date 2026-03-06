using System.Text;
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
    public class StringConcatenationBenchmarks
    {
        private class Config : ManualConfig
        {
            public Config()
            {
                SummaryStyle =
                    SummaryStyle.Default.WithRatioStyle(RatioStyle.Percentage);
            }
        }

        private readonly string _word = "hello";

        [Params(10, 100, 1000)]
        public int Iterations;

        // Consumes result to prevent JIT dead code elimination
        public string _sink = string.Empty;

        [Benchmark(Baseline = true)]
        public void PlusOperator()
        {
            string result = string.Empty;
            for (int i = 0; i < Iterations; i++)
                result += _word;
            _sink = result;
        }

        [Benchmark]
        public void StringBuilder()
        {
            var sb = new StringBuilder(capacity: _word.Length * Iterations);
            for (int i = 0; i < Iterations; i++)
                sb.Append(_word);
            _sink = sb.ToString();
        }

        [Benchmark]
        public void StringConcat()
        {
            var parts = new string[Iterations];
            for (int i = 0; i < Iterations; i++)
                parts[i] = _word;
            _sink = string.Concat(parts);
        }
    }
}

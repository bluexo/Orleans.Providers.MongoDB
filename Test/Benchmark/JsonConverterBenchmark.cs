using System;
using System.Collections.Generic;
using System.Text;

using Benchmark;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;

namespace Benchmark
{
    [CoreJob, RyuJitX64Job]
    public class JsonConverterBenchmark
    {
        [GlobalSetup]
        public void Setup()
        {

        }
    }
}

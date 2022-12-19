using Benchcosmoscli.Benchmarks;
using BenchmarkDotNet.Running;

Console.WriteLine("A Sample Benchmark Runner");
var summary = BenchmarkRunner.Run<CosmosBenchmark>();
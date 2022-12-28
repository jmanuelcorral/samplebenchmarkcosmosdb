using Benchcosmoscli.Benchmarks;
using Benchcosmoscli.Helpers;
using Benchcosmoscli.Model;
using Benchcosmoscli.Seeders;
using BenchmarkDotNet.Running;
using Bogus;

Console.WriteLine("A Sample Benchmark Runner");

//await InvoiceSeeder.SeedData();
await OrdersTreeSeeder.SeedData();
//var summary = BenchmarkRunner.Run<CosmosBenchmark>();
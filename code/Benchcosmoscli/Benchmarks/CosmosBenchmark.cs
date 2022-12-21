using Benchcosmoscli.Config;
using Benchcosmoscli.Helpers;
using Benchcosmoscli.Model;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Bogus;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Benchcosmoscli.Benchmarks
{
    [Config(typeof(AntiVirusFriendlyConfig))]
    public class CosmosBenchmark
    {
        private CosmosTestRun<InvoiceWithLines> _rootResult;
        private CosmosTestRun<InvoiceLines> _linesResult;

        //[Params(1000, 10000, 100000)]
        [Params(1000)]
        public int N; //De momento no lo usamos

        [IterationCleanup(Target = "QueryRoot")]
        public void QueryRootOutput() => File.WriteAllText($"rtus-size.QueryRoot.{N}.txt", _rootResult._requestsConsumed.ToString());

        [Benchmark]
        public async Task QueryRoot()
        {
            _rootResult =  await CosmosHelpers.QueryItems<InvoiceWithLines>(databaseName: "indicosmos", containerName: "Invoices2", query: new QueryDefinition($"SELECT * FROM c.data"));
        }


        [IterationCleanup(Target = "QueryLines")]
        public void QueryLinesOutput() => File.WriteAllText($"rtus-size.QueryLines.{N}.txt", _linesResult._requestsConsumed.ToString());

        [Benchmark]
        public async Task QueryLines()
        {
            _linesResult = await CosmosHelpers.QueryItems<InvoiceLines>(databaseName: "indicosmos", containerName: "Invoices2", query: new QueryDefinition("SELECT VALUE v FROM v IN root.data.lines"));
            
        }
    }
}

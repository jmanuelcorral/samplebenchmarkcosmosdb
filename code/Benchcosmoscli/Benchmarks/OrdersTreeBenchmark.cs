using Benchcosmoscli.Config;
using Benchcosmoscli.Helpers;
using Benchcosmoscli.Model;
using BenchmarkDotNet.Attributes;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchcosmoscli.Benchmarks
{

    [Config(typeof(AntiVirusFriendlyConfig))]
    public class OrdersTreeBenchmark
    {
        private CosmosTestRun<Pedido> _rootResult;
        private CosmosTestRun<LineaPedido> _linesResult;

        [Params(2000)]
        public int N; //De momento no lo usamos

        [IterationCleanup(Target = "QueryRoot")]
        public void QueryRootOutput() => File.WriteAllText($"rtus-size.QueryRoot.{N}.txt", _rootResult._requestsConsumed.ToString());

        [Benchmark]
        public async Task QueryRoot()
        {
            _rootResult = await CosmosHelpers.QueryItems<Pedido>(databaseName: "indicosmos", containerName: "Pedidos", query: new QueryDefinition($"SELECT * FROM c"));
        }


        [IterationCleanup(Target = "QueryLines")]
        public void QueryLinesOutput() => File.WriteAllText($"rtus-size.QueryLines.{N}.txt", _linesResult._requestsConsumed.ToString());

        [Benchmark]
        public async Task QueryLines()
        {
            _linesResult = await CosmosHelpers.QueryItems<LineaPedido>(databaseName: "indicosmos", containerName: "Pedidos", query: new QueryDefinition("SELECT * FROM c.lineasPedido"));

        }
    }
}

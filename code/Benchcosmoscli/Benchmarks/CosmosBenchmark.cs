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
        private List<InvoiceWithLines> _invoiceData;
        private int _numberOfInvoiceLinesPerInvoice = 30;
        private int _numberOfInvoices = 20;

        private CosmosTestRun<InvoiceWithLines> _rootResult;
        private CosmosTestRun<InvoiceLines> _linesResult;

        private class Config : ManualConfig
        {
            public Config() => AddColumn(new RequestChargeColumn());
        }

        [GlobalSetup]
        public async Task Setup()
        {
            Faker<InvoiceLines> FakerInvoiceLinesDataGenerator = new Faker<InvoiceLines>()
                .RuleFor(p => p.Id, f => Guid.NewGuid())
                .RuleFor(p => p.Description, f => f.Commerce.Product())
                .RuleFor(p => p.Quantity, f => f.PickRandom<int>(0, 1, 5, 6, 7, 9, 11))
                .RuleFor(p => p.Price, f => f.Random.Number(1, 500));
            
            Faker<InvoiceWithLines> FakeInvoiceDataGenerator = new Faker<InvoiceWithLines>()
           .RuleFor(p => p.Id, f => Guid.NewGuid())
           .RuleFor(p => p.CustomerName, f => $"{f.Name.FirstName()} {f.Name.LastName()}")
           .RuleFor(p => p.CustomerAddress, f => f.Address.FullAddress())
           .RuleFor(p => p.lines, f => FakerInvoiceLinesDataGenerator.Generate(_numberOfInvoiceLinesPerInvoice));

            _invoiceData = FakeInvoiceDataGenerator.Generate(_numberOfInvoices).ToList();
            //await CosmosHelpers.InsertTransacctionalBatch("indicosmos", "Invoices", "id", _invoiceData);
            
        }

        [Params(1000, 10000, 100000)]
        public int N;

        [IterationCleanup(Target = "QueryRoot")]
        public void QueryRootOutput() => File.WriteAllText($"rtus-size.QueryRoot.{N}.txt", _rootResult._requestsConsumed.ToString());

        [Benchmark]
        public async Task QueryRoot()
        {
            _rootResult =  await CosmosHelpers.QueryItems<InvoiceWithLines>(databaseName: "indicosmos", containerName: "Invoices", query: new QueryDefinition("SELECT * FROM c"));
        }


        [IterationCleanup(Target = "QueryLines")]
        public void QueryLinesOutput() => File.WriteAllText($"rtus-size.QueryLines.{N}.txt", _linesResult._requestsConsumed.ToString());

        [Benchmark]
        public async Task QueryLines()
        {
            _linesResult = await CosmosHelpers.QueryItems<InvoiceLines>(databaseName: "indicosmos", containerName: "Invoices", query: new QueryDefinition("SELECT * FROM c.lines"));
            
        }
    }
}

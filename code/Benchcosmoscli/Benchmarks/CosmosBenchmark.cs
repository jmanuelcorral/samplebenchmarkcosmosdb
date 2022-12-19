using Benchcosmoscli.Helpers;
using Benchcosmoscli.Model;
using BenchmarkDotNet.Attributes;
using Bogus;
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


        [GlobalSetup]
        public void Setup()
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

            //Console.WriteLine(JsonConvert.SerializeObject(_invoiceData, Formatting.Indented));
        }


        [Benchmark]
        public void InsertByRegistry()
        {
            for (int i = 0; i < _numberOfInvoices; i++)
                CosmosHelpers.InsertItem("indicosmos", "Invoices", _invoiceData[i]);
        }

        [Benchmark]
        public void InsertBatch()
        {
            CosmosHelpers.InsertTransacctionalBatch("indicosmos", "Invoices", "id",_invoiceData);
        }
    }
}

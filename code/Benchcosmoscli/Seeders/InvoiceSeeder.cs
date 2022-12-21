using Benchcosmoscli.Helpers;
using Benchcosmoscli.Model;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchcosmoscli.Seeders
{
    public static class InvoiceSeeder
    {

        public static int Batchsize=> 50;
        public static int SeedRounds => 10;
        public static async Task SeedData()
        {
            Faker<InvoiceLines> FakerInvoiceLinesDataGenerator = new Faker<InvoiceLines>()
             .RuleFor(p => p.id, f => Guid.NewGuid().ToString())
             .RuleFor(p => p.Description, f => f.Commerce.Product())
             .RuleFor(p => p.Quantity, f => f.PickRandom<int>(0, 1, 5, 6, 7, 9, 11))
             .RuleFor(p => p.Price, f => f.Random.Number(1, 500));

            Faker<InvoiceWithLines> FakeInvoiceDataGenerator = new Faker<InvoiceWithLines>()
            .RuleFor(p => p.id, f => Guid.NewGuid().ToString())
            .RuleFor(p => p.CustomerName, f => $"{f.Name.FirstName()} {f.Name.LastName()}")
            .RuleFor(p => p.CustomerAddress, f => f.Address.FullAddress())
            .RuleFor(p => p.InvoiceDate, f => DateTime.UtcNow)
            .RuleFor(p => p.lines, f => FakerInvoiceLinesDataGenerator.Generate(100));

            Faker<InvoiceDAO> FakerInvoiceDAO = new Faker<InvoiceDAO>()
                .RuleFor(p => p.id, f => Guid.NewGuid().ToString())
                .RuleFor(p => p.data, f => FakeInvoiceDataGenerator.Generate(1)[0]);

            await CosmosHelpers.CreateContainer("indicosmos", "Invoices2", "/partitionKey");

            for (var i = 0; i <= SeedRounds; i++)
            {
                var myData = FakerInvoiceDAO.Generate(Batchsize).ToList();
                await CosmosHelpers.InsertTransacctionalBatch<InvoiceDAO, InvoiceWithLines>("indicosmos", "Invoices2", myData);
            }
        }
    }
}

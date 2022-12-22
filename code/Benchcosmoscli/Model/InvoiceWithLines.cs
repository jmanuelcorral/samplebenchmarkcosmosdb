using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Benchcosmoscli.Model
{

    public interface IDataObject
    {
        string id { get; set; }
        string partitionKey { get; }
        int ttl { get; set; }
    }

    public class InvoiceWithLines : IDataObject
    {
        public string id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string partitionKey { get; set; } = "INVOICE";
        public List<InvoiceLines> lines { get; set; } = new List<InvoiceLines>();
        public int ttl { get; set; } = -1;
    }

    public class InvoiceLines
    {
        public int Quantity { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

    }
}

using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Benchcosmoscli.Model
{

    public interface IDataObject<T>
    {
        string id { get; set; }
        string partitionKey { get; }
        string type { get; }
        T data { get; set; }
        string etag { get; set; }
        int ttl { get; set; }
    }

    public class InvoiceDAO : IDataObject<InvoiceWithLines>
    {
        public string id { get; set; }

        public string partitionKey => "invoice";

        public string type => "invoice";

        public InvoiceWithLines data { get; set; }

        public string etag { get; set; }

        public int ttl { get; set; } = -1;
        
    }

    public class InvoiceWithLines
    {
        
        public string id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }

        public string partitionKey { get; set; } = "INVOICE";
        public DateTime InvoiceDate { get; set; }

        public List<InvoiceLines> lines { get; set; } = new List<InvoiceLines>();
    }

    public class InvoiceLines
    {
        [JsonPropertyName("id")]
        public string id { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchcosmoscli.Model
{
    public class InvoiceWithLines
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        
        public DateTime InvoiceDate { get; set; }

        public List<InvoiceLines> lines { get; set; } = new List<InvoiceLines>();
    }

    public class InvoiceLines
    {
        public Guid Id { get; set; }
        public Guid InvoiceId { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

    }
}

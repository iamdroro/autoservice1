using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autocervice.Report
{
    public class OrderServiceDetail
    {
        public string ServiceName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice => Quantity * Price;
        public string PerformerName { get; set; }
    }
}

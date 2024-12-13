using Autocervice.Models;
using Autocervice.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autocervice.Report
{
    public class WorkOrderReport
    {
        public Order Order { get; set; }
        public List<OrderServiceDetail> Services { get; set; }
        public List<OrderPartDetail> Parts { get; set; }
        public decimal TotalServiceCost => Services.Sum(s => s.TotalPrice);
        public decimal TotalPartCost => Parts.Sum(p => p.TotalPrice);
        public decimal GrandTotal => TotalServiceCost + TotalPartCost;
    }
}

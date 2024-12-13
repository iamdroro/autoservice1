using Autocervice.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autocervice.Models
{
    public class OrderPart 
    {
        public int OrderID { get; set; }
        public int PartID { get; set; }
        public int Quantity { get; set; }
        public DateTime DateAdded { get; set; }

        // Навигационные свойства
        public Order Order { get; set; }
        public Part Part { get; set; }
    }
}
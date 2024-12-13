using Autocervice.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autocervice.Models
{
    public class Part : IEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool IsInStock { get; set; }
        public string Supplier { get; set; }
        public string TypeOfPart { get; set; } // "Для двигателя", "Тормоза", ...
        public string OEM { get; set; }

        // Навигационное свойство
        public List<OrderPart> OrderParts { get; set; }
    }

}


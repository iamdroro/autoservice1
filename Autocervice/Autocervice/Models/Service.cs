using Autocervice.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autocervice.Models
{
    public class Service : IEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public TimeSpan? Duration { get; set; }
        public string TypeOfService { get; set; } // "Техническое обслуживание", "Ремонт двигателя", ...

        // Навигационное свойство
        public List<OrderService> OrderServices { get; set; }
    }


}


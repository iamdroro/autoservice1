using Autocervice.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autocervice.Models
{
    public class WorkOrder : IEntity
    {
        public int ID { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string Status { get; set; } // "в процессе", "выполнен"
        public int OrderID { get; set; }
        public string WorkDescription { get; set; }

        // Навигационное свойство
        public Order Order { get; set; }
    }
}


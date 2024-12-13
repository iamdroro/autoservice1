using Autocervice.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autocervice.Models
{
    public class Assignment : IEntity
    {
        public int ID { get; set; }
        public int ManagerID { get; set; }
        public int OrderID { get; set; }
        public int PerformerID { get; set; }
        public DateTime AssignmentDate { get; set; }

        // Навигационные свойства
        public Manager Manager { get; set; }
        public Order Order { get; set; }
        public Performer Performer { get; set; }
    }
}


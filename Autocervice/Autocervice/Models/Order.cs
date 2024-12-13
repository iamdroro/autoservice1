using Autocervice.Interface;
using Autocervice.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autocervice.Models
{
    public class Order : IEntity { 
        public int ID { get; set; }
        public int ClientID { get; set; }
        public int PerformerID { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string Status { get; set; } // "в ожидании", "выполнен", "отменен"
        public string BodyNumber { get; set; }  // Номер кузова
        public string EngineNumber { get; set; }  // Номер двигателя
        // Навигационные свойства
        public Client Client { get; set; }
        public Performer Performer { get; set; }
        public List<OrderService> OrderServices { get; set; }
        public List<OrderPart> OrderParts { get; set; }
    }
}



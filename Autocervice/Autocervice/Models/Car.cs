using Autocervice.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autocervice.Models
{
    public class Car : IEntity
    {
        public int ID { get; set; }
        public int ClientID { get; set; }
        public string BrandModel { get; set; }
        public string BodyNumber { get; set; }
        public string EngineNumber { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set; }
        public DateTime? LastServiceDate { get; set; }
        public string Notes { get; set; }

        // Навигационное свойство
        public Client Client { get; set; }
    }
}

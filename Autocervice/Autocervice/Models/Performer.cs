using Autocervice.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autocervice.Models
{
    public class Performer : IEntity
    {
        public int ID { get; set; } // Идентификатор исполнителя
        public string Name { get; set; } // ФИО исполнителя
        public string Specialty { get; set; } // Специальность
        public string Phone { get; set; } // Контактный телефон
        public string Email { get; set; } // Электронная почта

        // Навигационное свойство для заказов
        public List<Order> Orders { get; set; }

        // Навигационное свойство для назначений
        public List<Assignment> Assignments { get; set; }
    }
}


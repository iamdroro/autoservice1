using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Autocervice.Models;
using System.Collections.ObjectModel;
using Autocervice.Services;
using System.Windows;
using Autocervice.ViewModels;

namespace Autocervice.ViewModels
{
    public class OrdersViewModel : BaseViewModel
    {
        private readonly Services.OrdersService _orderService;

        public ObservableCollection<Order> Orders { get; set; }

        public OrdersViewModel(Services.OrdersService orderService)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            Orders = new ObservableCollection<Order>(_orderService.GetAllOrders());
        }

        private Order _selectedOrder;

        public Order SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddOrderCommand => CreateCommand(() =>
        {
            var newOrder = new Order
            {
                ClientID = 1,
                PerformerID = 1,
                CreationDate = DateTime.Now,
                Status = "в ожидании"
            };
            _orderService.AddOrder(newOrder);
            Orders.Add(newOrder);
        });

        public ICommand DeleteOrderCommand => CreateCommand(param =>
        {
            if (param is Order order)
            {
                _orderService.DeleteOrder(order.ID);
                Orders.Remove(order);
            }
        }, param => param is Order);
    }
}



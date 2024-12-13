using Autocervice.Models;
using Autocervice.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Autocervice.ViewModels
{
      public class UniversalEditViewModel
    {
        private readonly DatabaseService _databaseService;
        private string _currentTable;
        public ObservableCollection<object> CurrentItems { get; private set; }

        public UniversalEditViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            CurrentItems = new ObservableCollection<object>();
        }

        // Метод для загрузки таблицы
        public void LoadTable(string tableName)
        {
            try
            {
                _currentTable = tableName;
                CurrentItems.Clear();

                switch (tableName)
                {
                    case "Clients":
                        foreach (var client in _databaseService.GetClients())
                            CurrentItems.Add(client);
                        break;
                    case "Cars":
                        foreach (var car in _databaseService.GetCars())
                            CurrentItems.Add(car);
                        break;
                    case "Orders":
                        foreach (var order in _databaseService.GetOrders())
                            CurrentItems.Add(order);
                        break;
                    case "Performers":
                        foreach (var performer in _databaseService.GetPerformers())
                            CurrentItems.Add(performer);
                        break;
                    case "Parts":
                        foreach (var part in _databaseService.GetParts())
                            CurrentItems.Add(part);
                        break;
                    case "Services":
                        foreach (var service in _databaseService.GetServices())
                            CurrentItems.Add(service);
                        break;
                    default:
                        MessageBox.Show("Неизвестная таблица.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки таблицы: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Метод для сохранения изменений
        public void SaveChanges()
        {
            try
            {
                foreach (var item in CurrentItems)
                {
                    switch (_currentTable)
                    {
                        case "Clients":
                            SaveOrUpdateClient(item as Client);
                            break;
                        case "Cars":
                            SaveOrUpdateCar(item as Car);
                            break;
                        case "Orders":
                            SaveOrUpdateOrder(item as Order);
                            break;
                        case "Performers":
                            SaveOrUpdatePerformer(item as Performer);
                            break;
                        case "Parts":
                            SaveOrUpdatePart(item as Part);
                            break;
                        case "Services":
                            SaveOrUpdateService(item as Service);
                            break;
                    }
                }

                MessageBox.Show("Изменения сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения изменений: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveOrUpdateClient(Client client)
        {
            if (client == null) return;
            if (client.ID == 0)
                _databaseService.AddClient(client);
            else
                _databaseService.UpdateClient(client);
        }

        private void SaveOrUpdateCar(Car car)
        {
            if (car == null) return;
            if (car.ID == 0)
                _databaseService.AddCar(car);
            else
                _databaseService.UpdateCar(car);
        }

        private void SaveOrUpdateOrder(Order order)
        {
            if (order == null) return;
            if (order.ID == 0)
                _databaseService.AddOrder(order);
            else
                _databaseService.UpdateOrder(order);
        }

        private void SaveOrUpdatePerformer(Performer performer)
        {
            if (performer == null) return;
            if (performer.ID == 0)
                _databaseService.AddPerformer(performer);
            else
                _databaseService.UpdatePerformer(performer);
        }

        private void SaveOrUpdatePart(Part part)
        {
            if (part == null) return;
            if (part.ID == 0)
                _databaseService.AddPart(part);
            else
                _databaseService.UpdatePart(part);
        }

        private void SaveOrUpdateService(Service service)
        {
            if (service == null) return;
            if (service.ID == 0)
                _databaseService.AddService(service);
            else
                _databaseService.UpdateService(service);
        }

        // Метод для удаления элемента
        public void DeleteItem(object item)
        {
            try
            {
                switch (_currentTable)
                {
                    case "Clients":
                        var client = item as Client;
                        if (client != null)
                            _databaseService.DeleteClient(client.ID);
                        break;
                    case "Cars":
                        var car = item as Car;
                        if (car != null)
                            _databaseService.DeleteCar(car.ID);
                        break;
                    case "Orders":
                        var order = item as Order;
                        if (order != null)
                            _databaseService.DeleteOrder(order.ID);
                        break;
                    case "Performers":
                        var performer = item as Performer;
                        if (performer != null)
                            _databaseService.DeletePerformer(performer.ID);
                        break;
                    case "Parts":
                        var part = item as Part;
                        if (part != null)
                            _databaseService.DeletePart(part.ID);
                        break;
                    case "Services":
                        var service = item as Service;
                        if (service != null)
                            _databaseService.DeleteService(service.ID);
                        break;
                }

                CurrentItems.Remove(item);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления элемента: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Метод для добавления нового элемента
        public void AddNewItem()
        {
            try
            {
                switch (_currentTable)
                {
                    case "Clients":
                        CurrentItems.Add(new Client { ID = 0 });
                        break;
                    case "Cars":
                        CurrentItems.Add(new Car { ID = 0 });
                        break;
                    case "Orders":
                        CurrentItems.Add(new Order { ID = 0 });
                        break;
                    case "Performers":
                        CurrentItems.Add(new Performer { ID = 0 });
                        break;
                    case "Parts":
                        CurrentItems.Add(new Part { ID = 0 });
                        break;
                    case "Services":
                        CurrentItems.Add(new Service { ID = 0 });
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления элемента: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

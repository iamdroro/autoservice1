using Autocervice.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autocervice.Services;
using System.Windows.Input;

namespace Autocervice.ViewModels
{
    public class ServicesViewModel : BaseViewModel
    {
        private readonly ServiceService _serviceService;

        public ObservableCollection<Service> Services { get; set; }

        public ServicesViewModel(ServiceService serviceService)
        {
            _serviceService = serviceService;
            Services = new ObservableCollection<Service>(_serviceService.GetAllServices());
        }

        private Service _selectedService;
        public Service SelectedService
        {
            get => _selectedService;
            set
            {
                _selectedService = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddServiceCommand => CreateCommand(() =>
        {
            var newService = new Service
            {
                Name = "Новая услуга",
                Description = "",
                Quantity = 1,
                Price = 1000,
                Duration = TimeSpan.FromHours(1),
                TypeOfService = "Техническое обслуживание"
            };
            _serviceService.AddService(newService);
            Services.Add(newService);
        });

        public ICommand DeleteServiceCommand => CreateCommand(param =>
        {
            if (param is Service service)
            {
                _serviceService.DeleteService(service.ID);
                Services.Remove(service);
            }
        }, param => param is Service);
    }
}


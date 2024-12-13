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
    public class CarsViewModel : BaseViewModel
    {
        private readonly CarService _carService;

        public ObservableCollection<Car> Cars { get; set; }

        public CarsViewModel(CarService carService)
        {
            _carService = carService;
            Cars = new ObservableCollection<Car>(_carService.GetAllCars());
        }

        private Car _selectedCar;
        public Car SelectedCar
        {
            get => _selectedCar;
            set
            {
                _selectedCar = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCarCommand => CreateCommand(() =>
        {
            var newCar = new Car
            {
                ClientID = 1, // Пример значения
                BrandModel = "Новая машина",
                BodyNumber = "000000",
                EngineNumber = "000000",
                Year = 2024,
                Mileage = 0,
                LastServiceDate = null,
                Notes = "Примечания"
            };
            _carService.AddCar(newCar);
            Cars.Add(newCar);
        });

        public ICommand DeleteCarCommand => CreateCommand(param =>
        {
            if (param is Car car)
            {
                _carService.DeleteCar(car.ID);
                Cars.Remove(car);
            }
        }, param => param is Car);
    }
}

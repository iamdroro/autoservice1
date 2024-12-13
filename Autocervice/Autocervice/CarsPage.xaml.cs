using Autocervice.Services;
using Autocervice.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Autocervice
{
    /// <summary>
    /// Логика взаимодействия для CarsPage.xaml
    /// </summary>
    public partial class CarsPage : Page
    {
        private CarService _carService;
        private CarsViewModel _carsViewModel;

        public CarsPage()
        {
            InitializeComponent();
            _carService = new CarService(new DatabaseService());
            _carsViewModel = new CarsViewModel(_carService);
            CarsDataGrid.ItemsSource = _carsViewModel.Cars;
        }
    }
}

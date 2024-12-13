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
    /// Логика взаимодействия для ServicesPage.xaml
    /// </summary>
    public partial class ServicesPage : Page
    {
        private ServiceService _serviceService;
        private ServicesViewModel _servicesViewModel;

        public ServicesPage()
        {
            InitializeComponent();
            _serviceService = new ServiceService(new DatabaseService());
            _servicesViewModel = new ServicesViewModel(_serviceService);
            ServicesDataGrid.ItemsSource = _servicesViewModel.Services;
        }
    }
}

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
    public partial class PerformersPage : Page
    {
        private PerformerService _performerService;
        private PerformersViewModel _performersViewModel;

        public PerformersPage()
        {
            InitializeComponent();
            _performerService = new PerformerService(new DatabaseService());
            _performersViewModel = new PerformersViewModel(_performerService);
            PerformersDataGrid.ItemsSource = _performersViewModel.Performers;
        }
    }
}
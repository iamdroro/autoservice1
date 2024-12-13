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
    /// Логика взаимодействия для PartsPage.xaml
    /// </summary>
    public partial class PartsPage : Page
    {
        private PartService _partService;
        private PartsViewModel _partsViewModel;

        public PartsPage()
        {
            InitializeComponent();
            _partService = new PartService(new DatabaseService());
            _partsViewModel = new PartsViewModel(_partService);
            PartsDataGrid.ItemsSource = _partsViewModel.Parts;
        }
    }
}

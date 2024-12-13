using Autocervice.Services;

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
using Autocervice.Models;
using Autocervice.ViewModels;


namespace Autocervice
{
    public partial class ClientsPage : Page
    {
        private ClientService _clientService;
        private ClientsViewModel _clientsViewModel;

        public ClientsPage()
        {
            InitializeComponent();
            _clientService = new ClientService(new DatabaseService());
            _clientsViewModel = new ClientsViewModel(_clientService);
            ClientsDataGrid.ItemsSource = _clientsViewModel.Clients;
        }
    }
}





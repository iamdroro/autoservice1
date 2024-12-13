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
    public class ClientsViewModel : BaseViewModel
    {
        private readonly ClientService _clientService;

        public ObservableCollection<Client> Clients { get; set; }

        public ClientsViewModel(ClientService clientService)
        {
            _clientService = clientService;
            Clients = new ObservableCollection<Client>(_clientService.GetClients());
        }

        private Client _selectedClient;
        public Client SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddClientCommand => CreateCommand(() =>
        {
            var newClient = new Client
            {
                Name = "Новый клиент",
                PhoneNumber = "",
                Email = "",
                Address = ""
            };
            _clientService.AddClient(newClient);
            Clients.Add(newClient);
        });

        public ICommand DeleteClientCommand => CreateCommand(param =>
        {
            if (param is Client client)
            {
                _clientService.DeleteClient(client.ID);
                Clients.Remove(client);
            }
        }, param => param is Client);
    }
}

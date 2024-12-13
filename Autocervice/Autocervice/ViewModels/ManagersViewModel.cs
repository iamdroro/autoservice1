using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autocervice.Models;
using Autocervice.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Autocervice.ViewModels
{
    public class ManagersViewModel : BaseViewModel
    {
        private readonly ManagerService _managerService;

        public ObservableCollection<Manager> Managers { get; set; }

        public ManagersViewModel(ManagerService managerService)
        {
            _managerService = managerService;
            Managers = new ObservableCollection<Manager>(_managerService.GetAllManagers());
        }

        private Manager _selectedManager;
        public Manager SelectedManager
        {
            get => _selectedManager;
            set
            {
                _selectedManager = value;
                OnPropertyChanged();
            }
        }

        // Команда для добавления нового менеджера
        public ICommand AddManagerCommand => CreateCommand(() =>
        {
            var newManager = new Manager
            {
                Login = "new_login",
                PasswordHash = "password_hash",
                FirstName = "Имя",
                LastName = "Фамилия",
                Role = "Менеджер"
            };
            _managerService.AddManager(newManager);
            Managers.Add(newManager);
        });

        // Команда для удаления менеджера
        public ICommand DeleteManagerCommand => CreateCommand(param =>
        {
            if (param is Manager manager)
            {
                _managerService.DeleteManager(manager.ID);
                Managers.Remove(manager);
            }
        }, param => param is Manager);
    }
}

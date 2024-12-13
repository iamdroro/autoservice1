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
    public class PartsViewModel : BaseViewModel
    {
        private readonly PartService _partService;

        public ObservableCollection<Part> Parts { get; set; }

        public PartsViewModel(PartService partService)
        {
            _partService = partService;
            Parts = new ObservableCollection<Part>(_partService.GetAllParts());
        }

        private Part _selectedPart;
        public Part SelectedPart
        {
            get => _selectedPart;
            set
            {
                _selectedPart = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddPartCommand => CreateCommand(() =>
        {
            var newPart = new Part
            {
                Name = "Новое комплектующее",
                Quantity = 10,
                Price = 1000,
                IsInStock = true,
                Supplier = "Поставщик",
                TypeOfPart = "Для двигателя"
            };
            _partService.AddPart(newPart);
            Parts.Add(newPart);
        });

        public ICommand DeletePartCommand => CreateCommand(param =>
        {
            if (param is Part part)
            {
                _partService.DeletePart(part.ID);
                Parts.Remove(part);
            }
        }, param => param is Part);
    }
}

using Autocervice.Models;
using Autocervice.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Autocervice.ViewModels
{
    public class PerformersViewModel : BaseViewModel
    {
        private readonly PerformerService _performerService;

        public ObservableCollection<Performer> Performers { get; set; }

        public PerformersViewModel(PerformerService performerService)
        {
            _performerService = performerService;
            Performers = new ObservableCollection<Performer>(_performerService.GetAllPerformers());
        }

        private Performer _selectedPerformer;
        public Performer SelectedPerformer
        {
            get => _selectedPerformer;
            set
            {
                _selectedPerformer = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddPerformerCommand => CreateCommand(() =>
        {
            var newPerformer = new Performer
            {
                Name = "Новый исполнитель",
                Specialty = "Специальность",
                Phone = "",
                Email = ""
            };
            _performerService.AddPerformer(newPerformer);
            Performers.Add(newPerformer);
        });

        public ICommand DeletePerformerCommand => CreateCommand(param =>
        {
            if (param is Performer performer)
            {
                _performerService.DeletePerformer(performer.ID);
                Performers.Remove(performer);
            }
        }, param => param is Performer);
    }
}

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
using System.Collections.ObjectModel;
using Autocervice.ViewModels;
using Autocervice.Models;
using Autocervice.Services;


namespace Autocervice
{
    public partial class UniversalEditPage : Page
    {
        private UniversalEditViewModel _viewModel;

        public UniversalEditPage()
        {
            InitializeComponent();

            // Инициализация ViewModel
            _viewModel = new UniversalEditViewModel(new DatabaseService());
            DataContext = _viewModel;
        }

        // Обработчик для выбора таблицы
        private void TableSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TableSelector.SelectedItem is ComboBoxItem selectedItem)
            {
                string tableName = selectedItem.Tag.ToString();
                _viewModel.LoadTable(tableName);
            }
        }

        // Обработчик для сохранения изменений
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SaveChanges();
        }

        // Обработчик для удаления элемента
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (UniversalDataGrid.SelectedItem != null)
            {
                _viewModel.DeleteItem(UniversalDataGrid.SelectedItem);
            }
        }

        // Обработчик для добавления нового элемента
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AddNewItem();
        }
    }
}
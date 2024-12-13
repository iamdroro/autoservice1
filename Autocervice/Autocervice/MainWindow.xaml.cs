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


namespace Autocervice
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Обработчик для кнопки "Клиенты"
        private void ClientsButton_Click(object sender, RoutedEventArgs e)
        {
            // Загружаем ClientsPage в Frame
            MainFrame.Navigate(new ClientsPage());
        }

        // Для других кнопок можно добавить аналогичные обработчики
        private void CarsButton_Click(object sender, RoutedEventArgs e)
        {
            // Например, переход на страницу с автомобилями
             MainFrame.Navigate(new CarsPage());
        }

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
             MainFrame.Navigate(new OrdersPage());
        }

        private void ServicesButton_Click(object sender, RoutedEventArgs e)
        {
           MainFrame.Navigate(new ServicesPage());
        }

        private void PartsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PartsPage());
        }
        private void PerformersButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PerformersPage());
        }
        private void EditTablesButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UniversalEditPage());
        }
    }
}

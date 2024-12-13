using Autocervice.Models;
using Autocervice.Report;
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
using OrderService = Autocervice.Models.OrderService;
using System.Diagnostics;


namespace Autocervice
{
    /// <summary>
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        private readonly Autocervice.Services.OrdersService _orderService;
        private readonly OrdersViewModel _ordersViewModel;

        public OrdersPage()
        {
            InitializeComponent();

            // Инициализируем сервисы
            _orderService = new Autocervice.Services.OrdersService(new DatabaseService());
            _ordersViewModel = new OrdersViewModel(_orderService);

            // Привязываем данные к DataGrid
            OrdersDataGrid.ItemsSource = _ordersViewModel.Orders;
        }

        private void GenerateWorkOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem is Order selectedOrder)
            {
                try
                {
                    // Инициализируем ReportService с передачей DatabaseService
                    var reportService = new ReportService(new DatabaseService());

                    // Генерируем отчет
                    var report = reportService.GenerateWorkOrderReport(selectedOrder.ID);

                    // Путь для сохранения отчета
                    string outputPath = $"WorkOrder_{report.Order.ID}.html";

                    // Генерация HTML-отчета
                    reportService.GenerateHtmlReport(report, outputPath);

                    // Открываем созданный HTML-отчет в браузере
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = outputPath,
                        UseShellExecute = true
                    });

                    MessageBox.Show($"Наряд-заказ успешно сгенерирован и сохранен по пути: {outputPath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при создании наряд-заказа: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для генерации наряд-заказа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}

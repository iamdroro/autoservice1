using Autocervice.Models;
using Autocervice.Report;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Autocervice.Services
{
    public class ReportService
    {
        private readonly DatabaseService _databaseService;

        public ReportService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public WorkOrderReport GenerateWorkOrderReport(int orderId)
        {
            var report = new WorkOrderReport();

            // Логика получения данных о заказе и связанных услугах и комплектующих
            // Пример:
            report.Order = _databaseService.GetOrders().FirstOrDefault(o => o.ID == orderId);
            report.Services = new List<OrderServiceDetail>(); // Получите список услуг
            report.Parts = new List<OrderPartDetail>(); // Получите список комплектующих

            return report;
        }

        public void GenerateHtmlReport(WorkOrderReport report, string outputPath)
        {
            string html = $@"
            <html>
            <head><title>Наряд-заказ #{report.Order.ID}</title></head>
            <body>
                <h1>Наряд-заказ #{report.Order.ID}</h1>
                <p>Клиент: {report.Order.ClientID}</p>
                <p>Статус: {report.Order.Status}</p>
                <h2>Услуги:</h2>
                <ul>";
            foreach (var service in report.Services)
            {
                html += $"<li>{service.ServiceName}: {service.Quantity} x {service.Price} = {service.TotalPrice}</li>";
            }
            html += "</ul><h2>Запчасти:</h2><ul>";
            foreach (var part in report.Parts)
            {
                html += $"<li>{part.PartName}: {part.Quantity} x {part.Price} = {part.TotalPrice}</li>";
            }
            html += "</ul></body></html>";

            System.IO.File.WriteAllText(outputPath, html);
        }
    }
}
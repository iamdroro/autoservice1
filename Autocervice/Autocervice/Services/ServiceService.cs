using Autocervice.Models;
using Autocervice.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;


namespace Autocervice.Services
{
    public class ServiceService
    {
        private readonly DatabaseService _databaseService;

        public ServiceService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public List<Service> GetAllServices()
        {
            var services = new List<Service>();

            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Услуга";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            services.Add(new Service
                            {
                                ID = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2),
                                Quantity = reader.GetInt32(3),
                                Price = reader.GetDecimal(4),
                                Duration = reader.GetTimeSpan(5),
                                TypeOfService = reader.GetString(6)
                            });
                        }
                    }
                }
            }

            return services;
        }

        public void AddService(Service service)
        {
            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
                string query = @"
                    INSERT INTO Услуга 
                    (Наименование_Услуги, Описание, Количество, Стоимость, Продолжительность_Выполнения, Тип_Услуги) 
                    VALUES 
                    (@Name, @Description, @Quantity, @Price, @Duration, @TypeOfService)";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Name", service.Name);
                    cmd.Parameters.AddWithValue("@Description", service.Description);
                    cmd.Parameters.AddWithValue("@Quantity", service.Quantity);
                    cmd.Parameters.AddWithValue("@Price", service.Price);
                    cmd.Parameters.AddWithValue("@Duration", service.Duration);
                    cmd.Parameters.AddWithValue("@TypeOfService", service.TypeOfService);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateService(Service service)
        {
            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
                string query = @"
                    UPDATE Услуга SET
                    Наименование_Услуги = @Name,
                    Описание = @Description,
                    Количество = @Quantity,
                    Стоимость = @Price,
                    Продолжительность_Выполнения = @Duration,
                    Тип_Услуги = @TypeOfService
                    WHERE Код_Услуги = @ID";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", service.ID);
                    cmd.Parameters.AddWithValue("@Name", service.Name);
                    cmd.Parameters.AddWithValue("@Description", service.Description);
                    cmd.Parameters.AddWithValue("@Quantity", service.Quantity);
                    cmd.Parameters.AddWithValue("@Price", service.Price);
                    cmd.Parameters.AddWithValue("@Duration", service.Duration);
                    cmd.Parameters.AddWithValue("@TypeOfService", service.TypeOfService);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteService(int serviceId)
        {
            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Услуга WHERE Код_Услуги = @ID";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", serviceId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

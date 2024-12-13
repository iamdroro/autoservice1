using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using Autocervice.Models;
using Autocervice.Interface;

namespace Autocervice.Services
{
    public class DatabaseService
    {
        public string connectionString = "Host=localhost;Port=5432;Database=autoservice1;Username=postgres;Password=2134;";

        public NpgsqlConnection GetConnection()
        {
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            return connection;
        }
        public List<T> GetAll<T>(string tableName) where T : IEntity, new()
        {
            var items = new List<T>();
            using (var connection = GetConnection())
            {
                string sql = $"SELECT * FROM {tableName}";
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        var properties = typeof(T).GetProperties();
                        while (reader.Read())
                        {
                            var item = new T();
                            foreach (var prop in properties)
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal(prop.Name)))
                                {
                                    prop.SetValue(item, reader[prop.Name]);
                                }
                            }
                            items.Add(item);
                        }
                    }
                }
            }
            return items;
        }

        public void Save<T>(string tableName, T entity) where T : IEntity
        {
            using (var connection = GetConnection())
            {
                var properties = typeof(T).GetProperties();
                if (entity.ID == 0)
                {
                    // Insert
                    var columns = string.Join(", ", properties.Where(p => p.Name != "ID").Select(p => p.Name));
                    var values = string.Join(", ", properties.Where(p => p.Name != "ID").Select(p => $"@{p.Name}"));
                    var sql = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        foreach (var prop in properties.Where(p => p.Name != "ID"))
                        {
                            cmd.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(entity) ?? DBNull.Value);
                        }
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    // Update
                    var updates = string.Join(", ", properties.Where(p => p.Name != "ID").Select(p => $"{p.Name} = @{p.Name}"));
                    var sql = $"UPDATE {tableName} SET {updates} WHERE ID = @ID";

                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        foreach (var prop in properties)
                        {
                            cmd.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(entity) ?? DBNull.Value);
                        }
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void Delete<T>(string tableName, int id) where T : IEntity
        {
            using (var connection = GetConnection())
            {
                string sql = $"DELETE FROM {tableName} WHERE ID = @ID";
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Методы для работы с клиентами
        public List<Client> GetClients()
        {
            var clients = new List<Client>();

            using (var connection = GetConnection())
            {
                string query = "SELECT * FROM Клиент";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clients.Add(new Client
                            {
                                ID = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                PhoneNumber = reader.GetString(2),
                                Email = reader.GetString(3),
                                Address = reader.GetString(4)
                            });
                        }
                    }
                }
            }

            return clients;
        }

        public void AddClient(Client client)
        {
            using (var connection = GetConnection())
            {
                string query = @"
                    INSERT INTO Клиент (ФИО_или_Название_Организации, Контактный_Номер_Телефона, Электронная_Почта, Адрес) 
                    VALUES (@Name, @Phone, @Email, @Address)";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Name", client.Name);
                    cmd.Parameters.AddWithValue("@Phone", client.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Email", client.Email);
                    cmd.Parameters.AddWithValue("@Address", client.Address);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateClient(Client client)
        {
            using (var connection = GetConnection())
            {
                string query = @"
                    UPDATE Клиент SET 
                        ФИО_или_Название_Организации = @Name,
                        Контактный_Номер_Телефона = @Phone,
                        Электронная_Почта = @Email,
                        Адрес = @Address
                    WHERE ID_Клиента = @ID";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", client.ID);
                    cmd.Parameters.AddWithValue("@Name", client.Name);
                    cmd.Parameters.AddWithValue("@Phone", client.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Email", client.Email);
                    cmd.Parameters.AddWithValue("@Address", client.Address);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteClient(int clientId)
        {
            using (var connection = GetConnection())
            {
                string query = "DELETE FROM Клиент WHERE ID_Клиента = @ID";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", clientId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Методы для работы с автомобилями
        public List<Car> GetCars()
        {
            var cars = new List<Car>();

            using (var connection = GetConnection())
            {
                string query = "SELECT * FROM Автомобиль";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cars.Add(new Car
                            {
                                ID = reader.GetInt32(0),
                                ClientID = reader.GetInt32(1),
                                BrandModel = reader.GetString(2),
                                BodyNumber = reader.GetString(3),
                                EngineNumber = reader.GetString(4),
                                Year = reader.GetInt32(5),
                                Mileage = reader.GetInt32(6),
                                LastServiceDate = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                                Notes = reader.GetString(8)
                            });
                        }
                    }
                }
            }

            return cars;
        }

        public void AddCar(Car car)
        {
            using (var connection = GetConnection())
            {
                string query = @"
                    INSERT INTO Автомобиль 
                    (ID_Клиента, Марка_и_Модель, Номер_Кузова, Номер_Двигателя, Год_Выпуска, Пробег, Дата_Последнего_Обслуживания, Особые_Замечания)
                    VALUES 
                    (@ClientID, @BrandModel, @BodyNumber, @EngineNumber, @Year, @Mileage, @LastServiceDate, @Notes)";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ClientID", car.ClientID);
                    cmd.Parameters.AddWithValue("@BrandModel", car.BrandModel);
                    cmd.Parameters.AddWithValue("@BodyNumber", car.BodyNumber);
                    cmd.Parameters.AddWithValue("@EngineNumber", car.EngineNumber);
                    cmd.Parameters.AddWithValue("@Year", car.Year);
                    cmd.Parameters.AddWithValue("@Mileage", car.Mileage);
                    cmd.Parameters.AddWithValue("@LastServiceDate", car.LastServiceDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Notes", car.Notes);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateCar(Car car)
        {
            using (var connection = GetConnection())
            {
                string query = @"
                    UPDATE Автомобиль SET 
                        ID_Клиента = @ClientID,
                        Марка_и_Модель = @BrandModel,
                        Номер_Кузова = @BodyNumber,
                        Номер_Двигателя = @EngineNumber,
                        Год_Выпуска = @Year,
                        Пробег = @Mileage,
                        Дата_Последнего_Обслуживания = @LastServiceDate,
                        Особые_Замечания = @Notes
                    WHERE ID = @ID";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", car.ID);
                    cmd.Parameters.AddWithValue("@ClientID", car.ClientID);
                    cmd.Parameters.AddWithValue("@BrandModel", car.BrandModel);
                    cmd.Parameters.AddWithValue("@BodyNumber", car.BodyNumber);
                    cmd.Parameters.AddWithValue("@EngineNumber", car.EngineNumber);
                    cmd.Parameters.AddWithValue("@Year", car.Year);
                    cmd.Parameters.AddWithValue("@Mileage", car.Mileage);
                    cmd.Parameters.AddWithValue("@LastServiceDate", car.LastServiceDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Notes", car.Notes);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteCar(int carId)
        {
            using (var connection = GetConnection())
            {
                string query = "DELETE FROM Автомобиль WHERE ID = @ID";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", carId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Manager> GetManagers()
        {
            var managers = new List<Manager>();

            using (var connection = GetConnection())
            {
                string query = "SELECT * FROM Менеджеры";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            managers.Add(new Manager
                            {
                                ID = reader.GetInt32(0),
                                Login = reader.GetString(1),
                                PasswordHash = reader.GetString(2),
                                FirstName = reader.GetString(3),
                                LastName = reader.GetString(4),
                                Role = reader.GetString(5)
                            });
                        }
                    }
                }
            }

            return managers;
        }

        public void AddManager(Manager manager)
        {
            using (var connection = GetConnection())
            {
                string query = @"
                    INSERT INTO Менеджеры (Логин, Хэш_Пароля, Имя, Фамилия, Роль) 
                    VALUES (@Login, @PasswordHash, @FirstName, @LastName, @Role)";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Login", manager.Login);
                    cmd.Parameters.AddWithValue("@PasswordHash", manager.PasswordHash);
                    cmd.Parameters.AddWithValue("@FirstName", manager.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", manager.LastName);
                    cmd.Parameters.AddWithValue("@Role", manager.Role);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateManager(Manager manager)
        {
            using (var connection = GetConnection())
            {
                string query = @"
                    UPDATE Менеджеры SET 
                        Логин = @Login,
                        Хэш_Пароля = @PasswordHash,
                        Имя = @FirstName,
                        Фамилия = @LastName,
                        Роль = @Role
                    WHERE ID = @ID";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", manager.ID);
                    cmd.Parameters.AddWithValue("@Login", manager.Login);
                    cmd.Parameters.AddWithValue("@PasswordHash", manager.PasswordHash);
                    cmd.Parameters.AddWithValue("@FirstName", manager.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", manager.LastName);
                    cmd.Parameters.AddWithValue("@Role", manager.Role);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteManager(int managerId)
        {
            using (var connection = GetConnection())
            {
                string query = "DELETE FROM Менеджеры WHERE ID = @ID";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", managerId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Методы для работы с исполнителями
        public List<Performer> GetPerformers()
        {
            var performers = new List<Performer>();

            using (var connection = GetConnection())
            {
                string query = "SELECT * FROM Исполнители";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            performers.Add(new Performer
                            {
                                ID = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Specialty = reader.GetString(2),
                                Phone = reader.GetString(3),
                                Email = reader.GetString(4)
                            });
                        }
                    }
                }
            }

            return performers;
        }

        public void AddPerformer(Performer performer)
        {
            using (var connection = GetConnection())
            {
                string query = @"
                    INSERT INTO Исполнители (ФИО_Исполнителя, Специальность, Телефон, Электронная_Почта) 
                    VALUES (@Name, @Specialty, @Phone, @Email)";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Name", performer.Name);
                    cmd.Parameters.AddWithValue("@Specialty", performer.Specialty);
                    cmd.Parameters.AddWithValue("@Phone", performer.Phone);
                    cmd.Parameters.AddWithValue("@Email", performer.Email);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdatePerformer(Performer performer)
        {
            using (var connection = GetConnection())
            {
                string query = @"
                    UPDATE Исполнители SET 
                        ФИО_Исполнителя = @Name,
                        Специальность = @Specialty,
                        Телефон = @Phone,
                        Электронная_Почта = @Email
                    WHERE ID_Исполнителя = @ID";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", performer.ID);
                    cmd.Parameters.AddWithValue("@Name", performer.Name);
                    cmd.Parameters.AddWithValue("@Specialty", performer.Specialty);
                    cmd.Parameters.AddWithValue("@Phone", performer.Phone);
                    cmd.Parameters.AddWithValue("@Email", performer.Email);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeletePerformer(int performerId)
        {
            using (var connection = GetConnection())
            {
                string query = "DELETE FROM Исполнители WHERE ID_Исполнителя = @ID";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", performerId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Методы для работы с услугами
        public List<Service> GetServices()
        {
            var services = new List<Service>();

            using (var connection = GetConnection())
            {
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
            using (var connection = GetConnection())
            {
                string query = @"
                    INSERT INTO Услуга (Наименование_Услуги, Описание, Количество, Стоимость, Продолжительность_Выполнения, Тип_Услуги) 
                    VALUES (@Name, @Description, @Quantity, @Price, @Duration, @TypeOfService)";

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
            using (var connection = GetConnection())
            {
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
            using (var connection = GetConnection())
            {
                string query = "DELETE FROM Услуга WHERE Код_Услуги = @ID";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", serviceId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<Order> GetOrders()
        {
            var orders = new List<Order>();

            using (var connection = GetConnection())
            {
                string query = "SELECT * FROM Заказ";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new Order
                            {
                                ID = reader.GetInt32(0),
                                ClientID = reader.GetInt32(1),
                                PerformerID = reader.GetInt32(2),
                                BodyNumber = reader.GetString(3),
                                EngineNumber = reader.GetString(4),
                                CreationDate = reader.GetDateTime(5),
                                CompletionDate = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                                Status = reader.GetString(7)
                            });
                        }
                    }
                }
            }

            return orders;
        }

        public void AddOrder(Order order)
        {
            using (var connection = GetConnection())
            {
                string query = @"
            INSERT INTO Заказ (ID_Клиента, ID_Исполнителя, Номер_Кузова, Номер_Двигателя, Дата_Создания, Дата_Исполнения, Статус_Заказа) 
            VALUES (@ClientID, @PerformerID, @BodyNumber, @EngineNumber, @CreationDate, @CompletionDate, @Status)";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ClientID", order.ClientID);
                    cmd.Parameters.AddWithValue("@PerformerID", order.PerformerID);
                    cmd.Parameters.AddWithValue("@BodyNumber", order.BodyNumber);
                    cmd.Parameters.AddWithValue("@EngineNumber", order.EngineNumber);
                    cmd.Parameters.AddWithValue("@CreationDate", order.CreationDate);
                    cmd.Parameters.AddWithValue("@CompletionDate", order.CompletionDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", order.Status);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateOrder(Order order)
        {
            using (var connection = GetConnection())
            {
                string query = @"
            UPDATE Заказ SET 
                ID_Клиента = @ClientID,
                ID_Исполнителя = @PerformerID,
                Номер_Кузова = @BodyNumber,
                Номер_Двигателя = @EngineNumber,
                Дата_Создания = @CreationDate,
                Дата_Исполнения = @CompletionDate,
                Статус_Заказа = @Status
            WHERE Номер_Заказа = @ID";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", order.ID);
                    cmd.Parameters.AddWithValue("@ClientID", order.ClientID);
                    cmd.Parameters.AddWithValue("@PerformerID", order.PerformerID);
                    cmd.Parameters.AddWithValue("@BodyNumber", order.BodyNumber);
                    cmd.Parameters.AddWithValue("@EngineNumber", order.EngineNumber);
                    cmd.Parameters.AddWithValue("@CreationDate", order.CreationDate);
                    cmd.Parameters.AddWithValue("@CompletionDate", order.CompletionDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", order.Status);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteOrder(int orderId)
        {
            using (var connection = GetConnection())
            {
                string query = "DELETE FROM Заказ WHERE Номер_Заказа = @ID";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", orderId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        // Методы для работы с нарядами-заказами
        public List<WorkOrder> GetWorkOrders()
        {
            var workOrders = new List<WorkOrder>();

            using (var connection = GetConnection())
            {
                string query = "SELECT * FROM Наряд_Заказ";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            workOrders.Add(new WorkOrder
                            {
                                ID = reader.GetInt32(0),
                                CreationDate = reader.GetDateTime(1),
                                CompletionDate = reader.IsDBNull(2) ? null : reader.GetDateTime(2),
                                Status = reader.GetString(3),
                                OrderID = reader.GetInt32(4),
                                WorkDescription = reader.GetString(5)
                            });
                        }
                    }
                }
            }

            return workOrders;
        }

        public void AddWorkOrder(WorkOrder workOrder)
        {
            using (var connection = GetConnection())
            {
                string query = @"
            INSERT INTO Наряд_Заказ (Дата_Создания, Дата_Выполнения, Статус, Номер_Заказа, Описание_Работ) 
            VALUES (@CreationDate, @CompletionDate, @Status, @OrderID, @WorkDescription)";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@CreationDate", workOrder.CreationDate);
                    cmd.Parameters.AddWithValue("@CompletionDate", workOrder.CompletionDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", workOrder.Status);
                    cmd.Parameters.AddWithValue("@OrderID", workOrder.OrderID);
                    cmd.Parameters.AddWithValue("@WorkDescription", workOrder.WorkDescription);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateWorkOrder(WorkOrder workOrder)
        {
            using (var connection = GetConnection())
            {
                string query = @"
            UPDATE Наряд_Заказ SET 
                Дата_Создания = @CreationDate,
                Дата_Выполнения = @CompletionDate,
                Статус = @Status,
                Номер_Заказа = @OrderID,
                Описание_Работ = @WorkDescription
            WHERE ID = @ID";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", workOrder.ID);
                    cmd.Parameters.AddWithValue("@CreationDate", workOrder.CreationDate);
                    cmd.Parameters.AddWithValue("@CompletionDate", workOrder.CompletionDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", workOrder.Status);
                    cmd.Parameters.AddWithValue("@OrderID", workOrder.OrderID);
                    cmd.Parameters.AddWithValue("@WorkDescription", workOrder.WorkDescription);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteWorkOrder(int workOrderId)
        {
            using (var connection = GetConnection())
            {
                string query = "DELETE FROM Наряд_Заказ WHERE ID = @ID";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", workOrderId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<Part> GetParts()
        {
            var parts = new List<Part>();

            using (var connection = GetConnection())
            {
                string sql = "SELECT * FROM Комплектующие";
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            parts.Add(new Part
                            {
                                ID = (int)reader["ID"],
                                Name = reader["Наименование"].ToString(),
                                Quantity = (int)reader["Количество"],
                                Price = (decimal)reader["Стоимость"],
                                IsInStock = (bool)reader["Наличие_на_Складе"],
                                Supplier = reader["Поставщик"].ToString(),
                                TypeOfPart = reader["Тип_Комплектующего"].ToString()
                            });
                        }
                    }
                }
            }

            return parts;
        }

        // Метод для получения комплектующего по ID
        public Part GetPartById(int id)
        {
            using (var connection = GetConnection())
            {
                string sql = "SELECT * FROM Комплектующие WHERE ID = @ID";
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Part
                            {
                                ID = (int)reader["ID"],
                                Name = reader["Наименование"].ToString(),
                                Quantity = (int)reader["Количество"],
                                Price = (decimal)reader["Стоимость"],
                                IsInStock = (bool)reader["Наличие_на_Складе"],
                                Supplier = reader["Поставщик"].ToString(),
                                TypeOfPart = reader["Тип_Комплектующего"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Метод для добавления комплектующего
        public void AddPart(Part part)
        {
            using (var connection = GetConnection())
            {
                string sql = "INSERT INTO Комплектующие (Наименование, Количество, Стоимость, Наличие_на_Складе, Поставщик, Тип_Комплектующего) " +
                             "VALUES (@Name, @Quantity, @Price, @IsInStock, @Supplier, @TypeOfPart)";
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@Name", part.Name);
                    cmd.Parameters.AddWithValue("@Quantity", part.Quantity);
                    cmd.Parameters.AddWithValue("@Price", part.Price);
                    cmd.Parameters.AddWithValue("@IsInStock", part.IsInStock);
                    cmd.Parameters.AddWithValue("@Supplier", part.Supplier);
                    cmd.Parameters.AddWithValue("@TypeOfPart", part.TypeOfPart);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Метод для обновления комплектующего
        public void UpdatePart(Part part)
        {
            using (var connection = GetConnection())
            {
                string sql = "UPDATE Комплектующие SET Наименование = @Name, Количество = @Quantity, Стоимость = @Price, " +
                             "Наличие_на_Складе = @IsInStock, Поставщик = @Supplier, Тип_Комплектующего = @TypeOfPart WHERE ID = @ID";
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", part.ID);
                    cmd.Parameters.AddWithValue("@Name", part.Name);
                    cmd.Parameters.AddWithValue("@Quantity", part.Quantity);
                    cmd.Parameters.AddWithValue("@Price", part.Price);
                    cmd.Parameters.AddWithValue("@IsInStock", part.IsInStock);
                    cmd.Parameters.AddWithValue("@Supplier", part.Supplier);
                    cmd.Parameters.AddWithValue("@TypeOfPart", part.TypeOfPart);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Метод для удаления комплектующего
        public void DeletePart(int id)
        {
            using (var connection = GetConnection())
            {
                string sql = "DELETE FROM Комплектующие WHERE ID = @ID";
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }



        // Методы для работы с OrderPart
        public List<OrderPart> GetOrderParts(int orderId)
        {
            var orderParts = new List<OrderPart>();

            using (var connection = GetConnection())
            {
                string sql = @"SELECT * FROM Заказ_Комплектующие WHERE Номер_Заказа = @OrderId";

                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@OrderId", orderId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orderParts.Add(new OrderPart
                            {
                                OrderID = (int)reader["Номер_Заказа"],
                                PartID = (int)reader["Код_Товара"],
                                Quantity = (int)reader["Количество"],
                                DateAdded = (DateTime)reader["Дата_Добавления"]
                            });
                        }
                    }
                }
            }

            return orderParts;
        }

        public void AddOrderPart(OrderPart orderPart)
        {
            using (var connection = GetConnection())
            {
                string sql = @"INSERT INTO Заказ_Комплектующие (Номер_Заказа, Код_Товара, Количество, Дата_Добавления) 
                          VALUES (@OrderId, @PartId, @Quantity, @DateAdded)";

                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@OrderId", orderPart.OrderID);
                    cmd.Parameters.AddWithValue("@PartId", orderPart.PartID);
                    cmd.Parameters.AddWithValue("@Quantity", orderPart.Quantity);
                    cmd.Parameters.AddWithValue("@DateAdded", orderPart.DateAdded);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteOrderPart(int orderId, int partId)
        {
            using (var connection = GetConnection())
            {
                string sql = "DELETE FROM Заказ_Комплектующие WHERE Номер_Заказа = @OrderId AND Код_Товара = @PartId";

                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    cmd.Parameters.AddWithValue("@PartId", partId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Методы для работы с OrderService
        public List<OrderService> GetOrderServices(int orderId)
        {
            var orderServices = new List<OrderService>();

            using (var connection = GetConnection())
            {
                string sql = @"SELECT * FROM Заказ_Услуга WHERE Номер_Заказа = @OrderId";

                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@OrderId", orderId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orderServices.Add(new OrderService
                            {
                                OrderID = (int)reader["Номер_Заказа"],
                                ServiceID = (int)reader["Код_Услуги"],
                                Quantity = (int)reader["Количество"],
                                DateAdded = (DateTime)reader["Дата_Добавления"]
                            });
                        }
                    }
                }
            }

            return orderServices;
        }

        public void AddOrderService(OrderService orderService)
        {
            using (var connection = GetConnection())
            {
                string sql = @"INSERT INTO Заказ_Услуга (Номер_Заказа, Код_Услуги, Количество, Дата_Добавления) 
                          VALUES (@OrderId, @ServiceId, @Quantity, @DateAdded)";

                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@OrderId", orderService.OrderID);
                    cmd.Parameters.AddWithValue("@ServiceId", orderService.ServiceID);
                    cmd.Parameters.AddWithValue("@Quantity", orderService.Quantity);
                    cmd.Parameters.AddWithValue("@DateAdded", orderService.DateAdded);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteOrderService(int orderId, int serviceId)
        {
            using (var connection = GetConnection())
            {
                string sql = "DELETE FROM Заказ_Услуга WHERE Номер_Заказа = @OrderId AND Код_Услуги = @ServiceId";

                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    cmd.Parameters.AddWithValue("@ServiceId", serviceId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

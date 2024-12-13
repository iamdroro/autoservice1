using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autocervice.Models;
using Npgsql;


namespace Autocervice.Services
{
    public class ManagerService
    {
        private readonly DatabaseService _databaseService;

        public ManagerService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public List<Manager> GetAllManagers()
        {
            var managers = new List<Manager>();

            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
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

        public Manager GetManagerByLogin(string login)
        {
            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Менеджеры WHERE Логин = @Login";
                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Login", login);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Manager
                            {
                                ID = reader.GetInt32(0),
                                Login = reader.GetString(1),
                                PasswordHash = reader.GetString(2),
                                FirstName = reader.GetString(3),
                                LastName = reader.GetString(4),
                                Role = reader.GetString(5)
                            };
                        }
                    }
                }
            }

            return null;
        }
        public void AddManager(Manager manager)
        {
            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
                var command = new NpgsqlCommand("INSERT INTO Менеджеры (Логин, Хэш_Пароля, Имя, Фамилия, Роль) VALUES (@Login, @PasswordHash, @FirstName, @LastName, @Role)", connection);
                command.Parameters.AddWithValue("@Login", manager.Login);
                command.Parameters.AddWithValue("@PasswordHash", manager.PasswordHash);
                command.Parameters.AddWithValue("@FirstName", manager.FirstName);
                command.Parameters.AddWithValue("@LastName", manager.LastName);
                command.Parameters.AddWithValue("@Role", manager.Role);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteManager(int managerId)
        {
            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
                var command = new NpgsqlCommand("DELETE FROM Менеджеры WHERE ID = @ID", connection);
                command.Parameters.AddWithValue("@ID", managerId);
                command.ExecuteNonQuery();
            }
        }

    }
}

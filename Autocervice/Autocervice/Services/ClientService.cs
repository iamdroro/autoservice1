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
    public class ClientService
    {
        private readonly DatabaseService _databaseService;

        public ClientService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public List<Client> GetClients()
        {
            var clients = new List<Client>();

            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
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
            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
                string query = @"
                    INSERT INTO Клиент 
                    (ФИО_или_Название_Организации, Контактный_Номер_Телефона, Электронная_Почта, Адрес) 
                    VALUES 
                    (@Name, @PhoneNumber, @Email, @Address)";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Name", client.Name);
                    cmd.Parameters.AddWithValue("@PhoneNumber", client.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Email", client.Email);
                    cmd.Parameters.AddWithValue("@Address", client.Address);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateClient(Client client)
        {
            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
                string query = @"
                    UPDATE Клиент SET
                    ФИО_или_Название_Организации = @Name,
                    Контактный_Номер_Телефона = @PhoneNumber,
                    Электронная_Почта = @Email,
                    Адрес = @Address
                    WHERE ID_Клиента = @ID";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", client.ID);
                    cmd.Parameters.AddWithValue("@Name", client.Name);
                    cmd.Parameters.AddWithValue("@PhoneNumber", client.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Email", client.Email);
                    cmd.Parameters.AddWithValue("@Address", client.Address);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteClient(int id)
        {
            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Клиент WHERE ID_Клиента = @ID";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

using Autocervice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autocervice.Models;
using Npgsql;
using System;
using System.Collections.Generic;

namespace Autocervice.Services
{
    public class PerformerService
    {
        private readonly DatabaseService _databaseService;

        public PerformerService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public List<Performer> GetAllPerformers()
        {
            var performers = new List<Performer>();

            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
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
            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
                string query = @"
                    INSERT INTO Исполнители 
                    (ФИО_Исполнителя, Специальность, Телефон, Электронная_Почта) 
                    VALUES 
                    (@Name, @Specialty, @Phone, @Email)";

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
            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
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
            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Исполнители WHERE ID_Исполнителя = @ID";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", performerId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

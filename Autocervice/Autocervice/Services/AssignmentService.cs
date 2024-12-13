using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autocervice.Models;
using Npgsql;


namespace Autocervice.Services
{
    public class AssignmentService
    {
        private readonly DatabaseService _databaseService;

        public AssignmentService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public List<Assignment> GetAssignmentsByManager(int managerId)
        {
            var assignments = new List<Assignment>();

            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT n.ID, n.ID_Менеджера, n.Номер_Заказа, n.ID_Исполнителя, n.Дата_Назначения
                    FROM Назначения n
                    WHERE n.ID_Менеджера = @ManagerId";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ManagerId", managerId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            assignments.Add(new Assignment
                            {
                                ID = reader.GetInt32(0),
                                ManagerID = reader.GetInt32(1),
                                OrderID = reader.GetInt32(2),
                                PerformerID = reader.GetInt32(3),
                                AssignmentDate = reader.GetDateTime(4)
                            });
                        }
                    }
                }
            }

            return assignments;
        }

        public void AddAssignment(Assignment assignment)
        {
            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
                string query = @"
                    INSERT INTO Назначения (ID_Менеджера, Номер_Заказа, ID_Исполнителя, Дата_Назначения)
                    VALUES (@ManagerID, @OrderID, @PerformerID, @AssignmentDate)";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ManagerID", assignment.ManagerID);
                    cmd.Parameters.AddWithValue("@OrderID", assignment.OrderID);
                    cmd.Parameters.AddWithValue("@PerformerID", assignment.PerformerID);
                    cmd.Parameters.AddWithValue("@AssignmentDate", assignment.AssignmentDate);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}


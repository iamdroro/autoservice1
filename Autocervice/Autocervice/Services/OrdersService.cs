using Autocervice.Models;
using Autocervice.Services;
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
    public class OrdersService
    {
        private readonly DatabaseService _databaseService;

        public OrdersService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public List<Order> GetAllOrders()
        {
            var orders = new List<Order>();

            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
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
                                CreationDate = reader.GetDateTime(3),
                                CompletionDate = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                                Status = reader.GetString(5)
                            });
                        }
                    }
                }
            }

            return orders;
        }

        public void AddOrder(Order order)
        {
            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
                string query = @"
                    INSERT INTO Заказ 
                    (ID_Клиента, ID_Исполнителя, Дата_Создания, Дата_Исполнения, Статус_Заказа) 
                    VALUES 
                    (@ClientID, @PerformerID, @CreationDate, @CompletionDate, @Status)";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ClientID", order.ClientID);
                    cmd.Parameters.AddWithValue("@PerformerID", order.PerformerID);
                    cmd.Parameters.AddWithValue("@CreationDate", order.CreationDate);
                    cmd.Parameters.AddWithValue("@CompletionDate", (object)order.CompletionDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", order.Status);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateOrder(Order order)
        {
            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
                string query = @"
                    UPDATE Заказ SET
                    ID_Клиента = @ClientID,
                    ID_Исполнителя = @PerformerID,
                    Дата_Создания = @CreationDate,
                    Дата_Исполнения = @CompletionDate,
                    Статус_Заказа = @Status
                    WHERE Номер_Заказа = @OrderID";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@OrderID", order.ID);
                    cmd.Parameters.AddWithValue("@ClientID", order.ClientID);
                    cmd.Parameters.AddWithValue("@PerformerID", order.PerformerID);
                    cmd.Parameters.AddWithValue("@CreationDate", order.CreationDate);
                    cmd.Parameters.AddWithValue("@CompletionDate", (object)order.CompletionDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", order.Status);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteOrder(int orderId)
        {
            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Заказ WHERE Номер_Заказа = @OrderID";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}



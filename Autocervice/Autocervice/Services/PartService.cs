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
    public class PartService
    {
        private readonly DatabaseService _databaseService;

        public PartService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public List<Part> GetAllParts()
        {
            var parts = new List<Part>();

            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Комплектующие";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            parts.Add(new Part
                            {
                                ID = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Quantity = reader.GetInt32(2),
                                Price = reader.GetDecimal(3),
                                IsInStock = reader.GetBoolean(4),
                                Supplier = reader.GetString(5),
                                TypeOfPart = reader.GetString(6),
                                OEM = reader.IsDBNull(7) ? null : reader.GetString(7)
                            });
                        }
                    }
                }
            }

            return parts;
        }

        public void AddPart(Part part)
        {
            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
                string query = @"
                    INSERT INTO Комплектующие 
                    (Наименование, Количество, Стоимость, Наличие_на_Складе, Поставщик, Тип_Комплектующего, OEM) 
                    VALUES 
                    (@Name, @Quantity, @Price, @IsInStock, @Supplier, @TypeOfPart, @OEM)";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Name", part.Name);
                    cmd.Parameters.AddWithValue("@Quantity", part.Quantity);
                    cmd.Parameters.AddWithValue("@Price", part.Price);
                    cmd.Parameters.AddWithValue("@IsInStock", part.IsInStock);
                    cmd.Parameters.AddWithValue("@Supplier", part.Supplier);
                    cmd.Parameters.AddWithValue("@TypeOfPart", part.TypeOfPart);
                    cmd.Parameters.AddWithValue("@OEM", (object)part.OEM ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdatePart(Part part)
        {
            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
                string query = @"
                    UPDATE Комплектующие SET
                    Наименование = @Name,
                    Количество = @Quantity,
                    Стоимость = @Price,
                    Наличие_на_Складе = @IsInStock,
                    Поставщик = @Supplier,
                    Тип_Комплектующего = @TypeOfPart,
                    OEM = @OEM
                    WHERE Код_Товара = @PartID";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@PartID", part.ID);
                    cmd.Parameters.AddWithValue("@Name", part.Name);
                    cmd.Parameters.AddWithValue("@Quantity", part.Quantity);
                    cmd.Parameters.AddWithValue("@Price", part.Price);
                    cmd.Parameters.AddWithValue("@IsInStock", part.IsInStock);
                    cmd.Parameters.AddWithValue("@Supplier", part.Supplier);
                    cmd.Parameters.AddWithValue("@TypeOfPart", part.TypeOfPart);
                    cmd.Parameters.AddWithValue("@OEM", (object)part.OEM ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeletePart(int partId)
        {
            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Комплектующие WHERE Код_Товара = @PartID";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@PartID", partId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}



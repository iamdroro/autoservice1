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
    public class CarService
    {
        private readonly DatabaseService _databaseService;

        public CarService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public List<Car> GetAllCars()
        {
            var cars = new List<Car>();

            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM Автомобиль";
                using (var cmd = new NpgsqlCommand(sql, connection))
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
                                LastServiceDate = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7),
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
            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
                string sql = @"
                    INSERT INTO Автомобиль 
                    (ID_Клиента, Марка_и_Модель, Номер_Кузова, Номер_Двигателя, Год_Выпуска, Пробег, Дата_Последнего_Обслуживания, Особые_Замечания) 
                    VALUES 
                    (@ClientID, @BrandModel, @BodyNumber, @EngineNumber, @Year, @Mileage, @LastServiceDate, @Notes)";

                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@ClientID", car.ClientID);
                    cmd.Parameters.AddWithValue("@BrandModel", car.BrandModel);
                    cmd.Parameters.AddWithValue("@BodyNumber", car.BodyNumber);
                    cmd.Parameters.AddWithValue("@EngineNumber", car.EngineNumber);
                    cmd.Parameters.AddWithValue("@Year", car.Year);
                    cmd.Parameters.AddWithValue("@Mileage", car.Mileage);
                    cmd.Parameters.AddWithValue("@LastServiceDate", (object)car.LastServiceDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Notes", car.Notes);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateCar(Car car)
        {
            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
                string sql = @"
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

                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", car.ID);
                    cmd.Parameters.AddWithValue("@ClientID", car.ClientID);
                    cmd.Parameters.AddWithValue("@BrandModel", car.BrandModel);
                    cmd.Parameters.AddWithValue("@BodyNumber", car.BodyNumber);
                    cmd.Parameters.AddWithValue("@EngineNumber", car.EngineNumber);
                    cmd.Parameters.AddWithValue("@Year", car.Year);
                    cmd.Parameters.AddWithValue("@Mileage", car.Mileage);
                    cmd.Parameters.AddWithValue("@LastServiceDate", (object)car.LastServiceDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Notes", car.Notes);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteCar(int id)
        {
            using (var connection = new NpgsqlConnection(_databaseService.connectionString))
            {
                connection.Open();
                string sql = "DELETE FROM Автомобиль WHERE ID = @ID";

                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}


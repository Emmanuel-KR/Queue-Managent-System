﻿using Npgsql;
using QueueSystem.Data;

namespace QueueSystem.Utilities
{
    public class CommonHelper
    {
        private IConfiguration _config;
        public CommonHelper(IConfiguration config)
        {
            _config = config;
        }
        public User GetUserByUserName(string query)
        {
            User user = new User();

            string connectionString = _config.GetConnectionString("DefaultConnection");

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string sql = query;
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                using (NpgsqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        user.Id = Convert.ToInt32(dataReader["id"]);
                        user.Name = dataReader["name"].ToString();
                        user.Password = dataReader["password"].ToString();
                        user.Role = dataReader["role"].ToString();
                        user.ServicePointId = Convert.ToInt32(dataReader["servicepointid"]);
                    }
                }
                connection.Close();
            }
            return user;
        }
    }
}

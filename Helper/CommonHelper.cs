using Npgsql;
using QueueSystem.Data;
using QueueSystem.Models;

namespace QueueSystem.Helper
{
    public class CommonHelper
    {
        private IConfiguration _config;
        

  

        public CommonHelper(IConfiguration config)
        {
            _config = config;
            

        }


        public int DMLTransaction(string query)
        {
            int Result;

            //string connectionString = _config.GetConnectionString("DefaultConnection");
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;Database=QMSDb;User Id=postgres;Password=post2023");

            //using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                string sql = query;
                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                Result = cmd.ExecuteNonQuery();


                conn.Close();
            }

            return Result;

        }



        public bool UserAlreadyExists(string query)
        {
           bool flag = false;
            //string connectionString = _config.GetConnectionString("DefaultConnection");
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;Database=QMSDb;User Id=postgres;Password=post2023");
            //using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                string sql = query;
                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.Text;

                
                
                NpgsqlDataReader dataReader = cmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        flag = true;
                    }
              
                
            }

            return flag;

        }
        
    }
}

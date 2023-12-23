using Npgsql;
using QueueSystem.Models;
using QueueSystem.Services;

namespace QueueSystem.Repositories
{
    
    public class AdminRepository : IAdminRepository
    {
        private const string CONNECTION_STRING = "Host=localhost:5432;" +
                          "Username=postgres;" +
                          "Password=post2023;" +
                          "Database=QMSDb";

        private const string _serviceProvidersTable = "users";
        private const string _servicePointTable = "servicepoints";

        private NpgsqlConnection connection;
        public AdminRepository()
        {
            connection = new NpgsqlConnection(CONNECTION_STRING);
            connection.Open();
        }
        public async Task<IEnumerable<ServiceProviderM>> GetServiceProviders()
        {
            List<ServiceProviderM> serviceProviders = new List<ServiceProviderM>();

            string commandText = $"SELECT * FROM {_serviceProvidersTable} WHERE role = 'Service Provider'";
            await using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, connection))
            await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    ServiceProviderM serviceProvider = ReadServiceProviders(reader);
                    serviceProviders.Add(serviceProvider);
                }
            return serviceProviders;
        }
        private static ServiceProviderM ReadServiceProviders(NpgsqlDataReader reader)
        {
            int? id = reader["id"] as int?;
            string name = reader["name"] as string;
            string role = reader["role"] as string;
            ServiceProviderM serviceProviders = new ServiceProviderM
            {
                Id = (int)id,
                Name = name
                
            };
            return serviceProviders;
        }
        public async Task<ServiceProviderM> GetServiceProviderDetails(int id)
        {
            string commandText = $"SELECT * FROM {_serviceProvidersTable} WHERE ID = @Id";
            await using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("Id", id);

                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                    {
                        ServiceProviderM ServiceProviderDetails = ReadServiceProviderDetails(reader);
                        return ServiceProviderDetails;
                    }
            }
            return null;
        }
        private static ServiceProviderM ReadServiceProviderDetails(NpgsqlDataReader reader)
        {
            int? id = reader["id"] as int?;
            string name = reader["name"] as string;
            string password = reader["password"] as string;
            string role = reader["role"] as string;
            ServiceProviderM ServiceProviderDetails = new ServiceProviderM
            {
                Id = (int)id,
                Name = name,
                Password = password
                
            };
            return ServiceProviderDetails;
        }
        public async Task CreateServiceProvider(ServiceProviderM serviceProvider)
        {
            string commandText = $"INSERT INTO {_serviceProvidersTable} (name, password, role, servicepointid) VALUES (@name, @password, 'Service Provider', @servicepointid)";

            await using (var cmd = new NpgsqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("name", serviceProvider.Name);
                cmd.Parameters.AddWithValue("password", serviceProvider.Password);
                cmd.Parameters.AddWithValue("servicepointid", serviceProvider.ServicepointId);

                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task UpdateServiceProvider(int id, ServiceProviderM serviceProvider)
        {
            var commandText = $@"UPDATE {_serviceProvidersTable} SET password = @password, servicepointid = @servicepointid WHERE id = @id";

            await using (var cmd = new NpgsqlCommand(commandText, connection))
            {               
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("password", serviceProvider.Password);
                cmd.Parameters.AddWithValue("servicepointid", serviceProvider.ServicepointId);

                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task DeleteServiceProvider(int id)
        {
            string commandText = $"DELETE FROM {_serviceProvidersTable} WHERE ID=(@p)";
            await using (var cmd = new NpgsqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("p", id);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<ServicePointM>> GetServicePoints()
        {
            List<ServicePointM> servicePoints = new List<ServicePointM>();

            string commandText = $"SELECT * FROM {_servicePointTable}";
            await using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, connection))
            await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    ServicePointM servicePoint = ReadServicePoints(reader);
                    servicePoints.Add(servicePoint);
                }
            return servicePoints;
        }
        private static ServicePointM ReadServicePoints(NpgsqlDataReader reader)
        {
            int? id = reader["id"] as int?;
            string name = reader["name"] as string;
            int? serviceproviderId = reader["serviceproviderId"] as int?;
            ServicePointM servicePoint = new ServicePointM
            {
                Id = (int)id,
                Name = name,
                ServiceProviderId = (int)serviceproviderId
            };
            return servicePoint;
        }
        public async Task<ServicePointM> GetServicePointDetails(int id)
        {
            string commandText = $"SELECT * FROM {_servicePointTable} WHERE ID = @Id";
            await using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("Id", id);

                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                    {
                        ServicePointM ServiceProviderDetails = ReadServicePointDetails(reader);
                        return ServiceProviderDetails;
                    }
            }
            return null;
        }
        private static ServicePointM ReadServicePointDetails(NpgsqlDataReader reader)
        {
            int? id = reader["id"] as int?;
            string name = reader["name"] as string;
            int? serviceproviderid = reader["serviceproviderid"] as int?;
            ServicePointM ServiceProviderDetails = new ServicePointM
            {
                Id = (int)id,
                Name = name,
                ServiceProviderId = (int)serviceproviderid
            };
            return ServiceProviderDetails;
        }
        public async Task CreateServicePoint(ServicePointM servicePoint)
        {
            string commandText = $"INSERT INTO {_servicePointTable} (name, serviceproviderid) VALUES (@name, @serviceproviderid)";

            await using (var cmd = new NpgsqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("name", servicePoint.Name);
                cmd.Parameters.AddWithValue("serviceproviderid", servicePoint.ServiceProviderId);

                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task UpdateServicePoint(int id, ServicePointM servicePoint)
        {
            var commandText = $@"UPDATE {_servicePointTable} SET  serviceproviderid = @serviceproviderid WHERE id = @id";

            await using (var cmd = new NpgsqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("serviceproviderid", servicePoint.ServiceProviderId);

                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task DeleteServicePoint(int id)
        {
            string commandText = $"DELETE FROM {_servicePointTable} WHERE ID=(@p)";
            await using (var cmd = new NpgsqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("p", id);
                await cmd.ExecuteNonQueryAsync();
            }
        }       
    }
}

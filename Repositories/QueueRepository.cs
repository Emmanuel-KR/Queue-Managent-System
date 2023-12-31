﻿using Npgsql;
using QueueSystem.Models;
using QueueSystem.Services;

namespace QueueSystem.Repositories
{
    public class QueueRepository : IQueueRepository
    {
        private const string CONNECTION_STRING = "Host=localhost:5432;" +
                          "Username=postgres;" +
                          "Password=post2023;" +
                          "Database=QMSDb";

        private const string _servicePointsTable = "servicepoints";
        private const string _queueTable = "queue";

        private NpgsqlConnection connection;
        private static NpgsqlConnection connection2;

        public QueueRepository()
        {
            connection = new NpgsqlConnection(CONNECTION_STRING);
            connection2 = new NpgsqlConnection(CONNECTION_STRING);

            connection.Open();
            connection2.Open();
        }
        public async Task<IEnumerable<ServicePointM>> GetServices()
        {
            List<ServicePointM> services = new List<ServicePointM>();

            string commandText = $"SELECT * FROM {_servicePointsTable}";
            await using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, connection))
            await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    ServicePointM service = ReadServices(reader);
                    services.Add(service);
                }
            return services;
        }
        private static ServicePointM ReadServices(NpgsqlDataReader reader)
        {
            int? id = reader["id"] as int?;
            string name = reader["name"] as string;
            int? serviceproviderId = reader["serviceproviderId"] as int?;
            ServicePointM service = new ServicePointM
            {
                Id = (int)id,
                Name = name,
                ServiceProviderId = (int)serviceproviderId,
            };
            return service;
        }
        public async Task AddCustomerToQueue(ServicePointM customer)
        {
            var status = 0;
            string commandText = $"INSERT INTO {_queueTable} (servicepointid, status) VALUES (@servicepointid, {status})";

            await using (var cmd = new NpgsqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("servicepointid", customer.Id);
                cmd.Parameters.AddWithValue("status", status);

                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task<IEnumerable<QueueM>> GetCalledCustomers()
        {
            List<QueueM> calledCustomers = new List<QueueM>();

            string commandText = $"SELECT * FROM {_queueTable} WHERE status = 2";
            await using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, connection))
            await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    QueueM calledCustomer = ReadCalledCustomers(reader);
                    calledCustomers.Add(calledCustomer);
                }
            return calledCustomers;
        }
        private static QueueM ReadCalledCustomers(NpgsqlDataReader reader)
        {
            int? calledCustomerId = reader["id"] as int?;
            int? servicePointId = reader["servicepointid"] as int?;
            QueueM calledCustomer = new QueueM
            {
                Id = (int)calledCustomerId,
                ServicePointId = (int)servicePointId
            };
            return calledCustomer;
        }
        public async Task<IEnumerable<QueueM>> GetWaitingCustomers(string userServingPointId)
        {
            List<QueueM> waitingCustomers = new List<QueueM>();

            string commandText = $"SELECT * FROM {_queueTable} WHERE servicepointid = {userServingPointId} AND status = 0 ORDER BY id ASC";
            await using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, connection))
            await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    QueueM waitingCustomer = ReadWaitingCustomers(reader);
                    waitingCustomers.Add(waitingCustomer);
                }
            return waitingCustomers;
        }
        private static QueueM ReadWaitingCustomers(NpgsqlDataReader reader)
        {
            int? id = reader["id"] as int?;
            DateTime createdat = (DateTime)reader["createdat"];
            QueueM waitingCustomers = new QueueM
            {
                Id = (int)id,
                CreatedAt = createdat
            };
            return waitingCustomers;
        }
        public async Task<QueueM> MyCurrentServingCustomer(string userServingPointId)
        {
            string commandText = $"SELECT * FROM {_queueTable} WHERE status = 2 AND servicepointid = {userServingPointId}";
            await using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("Id", userServingPointId);

                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                    {
                        QueueM MyCurrentCustomerDetails = ReadMyCurrentServingCustomerDetails(reader);
                        return MyCurrentCustomerDetails;
                    }
            }
            return null;
        }
        private static QueueM ReadMyCurrentServingCustomerDetails(NpgsqlDataReader reader)
        {
            int? myCurrentCustomerId = reader["id"] as int?;
            QueueM MyCurrentCustomerDetails = new QueueM
            {
                Id = (int)myCurrentCustomerId
            };
            return MyCurrentCustomerDetails;
        }
        public async Task<QueueM> UpdateOutGoingAndIncomingCustomerStatus(int outgoingCustomerId, string serviceProviderId)
        {
            //Update the current customer as served
            var commandText = $@"UPDATE {_queueTable} SET status = 3, completedat = NOW() WHERE id = @id";
            await using (var cmd = new NpgsqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("id", outgoingCustomerId);

                await cmd.ExecuteNonQueryAsync();
            }

            //Get id of the next customer to be served
            string commandText2 = $"SELECT * FROM {_queueTable} WHERE status = 0 AND servicepointid = {serviceProviderId} ORDER BY id ASC LIMIT 1  ";

            await using (NpgsqlCommand cmd = new NpgsqlCommand(commandText2, connection))
            {
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                    {
                        QueueM IncomingCustomerDetails = ReadIncomingCustomerId(reader);
                        return IncomingCustomerDetails;
                    }
            }
            return null;
        }
        private static QueueM ReadIncomingCustomerId(NpgsqlDataReader reader)
        {
            int? incomingCustomerId = reader["id"] as int?;
            QueueM IncomingCustomerId = new QueueM
            {
                Id = (int)incomingCustomerId
            };
            UpdateIncomingCustomerStatus(IncomingCustomerId.Id);
            return IncomingCustomerId;
        }
        private static async void UpdateIncomingCustomerStatus(int? incomingCustomerId)
        {
            var commandText = $@"UPDATE {_queueTable} SET status = 2, updatedat = NOW() WHERE id = @id";
            await using (var cmd = new NpgsqlCommand(commandText, connection2))
            {
                cmd.Parameters.AddWithValue("id", incomingCustomerId);

                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task<QueueM> GetCurentlyCalledNumber(string serviceProviderId)
        {
            //Get id of the next customer to be served
            string commandText2 = $"SELECT * FROM {_queueTable} WHERE status = 2 AND servicepointid = {serviceProviderId}  ";

            await using (NpgsqlCommand cmd = new NpgsqlCommand(commandText2, connection))
            {
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                    {
                        QueueM CurrentlyCalledCustomerDetails = ReadCurrentlyCalledNumber(reader);
                        return CurrentlyCalledCustomerDetails;
                    }
            }
            return null;
        }
        private static QueueM ReadCurrentlyCalledNumber(NpgsqlDataReader reader)
        {
            int? currentlyCalledCustomerId = reader["id"] as int?;
            QueueM CurrentlyCalledCustomerId = new QueueM
            {
                Id = (int)currentlyCalledCustomerId
            };
            return CurrentlyCalledCustomerId;
        }
        public async Task<QueueM> MarkNumberASNoShow(string serviceProviderId)
        {
            QueueM customerIdToMarkAsFinished = await MyCurrentServingCustomer(serviceProviderId);

            //Update the current customer as served
            var commandText = $@"UPDATE {_queueTable} SET status = 4 , updatedat= NULL, completedat = NULL WHERE id = @id";
            await using (var cmd = new NpgsqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("id", customerIdToMarkAsFinished.Id);

                await cmd.ExecuteNonQueryAsync();
            }
            return null;
        }
        public async Task<QueueM> MarkNumberASFinished(string serviceProviderId)
        {
            QueueM customerIdToMarkAsFinished = await MyCurrentServingCustomer(serviceProviderId);

            //Update the current customer as served
            var commandText = $@"UPDATE {_queueTable} SET status = 3 , completedat = NOW() WHERE id = @id";
            await using (var cmd = new NpgsqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("id", customerIdToMarkAsFinished.Id);

                await cmd.ExecuteNonQueryAsync();
            }
            return null;
        }
        public async Task<QueueM> TransferNumber(string serviceProviderId, int servicePointid)
        {
            QueueM customerIdToMarkAsFinished = await MyCurrentServingCustomer(serviceProviderId);

            //Update the current customer as served
            var commandText = $@"UPDATE {_queueTable} SET servicepointid = {servicePointid}, status = 0, updatedat= NULL, completedat = NULL WHERE id = @id";
            await using (var cmd = new NpgsqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("id", customerIdToMarkAsFinished.Id);

                await cmd.ExecuteNonQueryAsync();
            }
            return null;
        }
    }
}

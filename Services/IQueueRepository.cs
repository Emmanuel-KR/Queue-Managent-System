
using QueueSystem.Models;

namespace QueueSystem.Services
{
    public interface IQueueRepository
    {
        Task<IEnumerable<ServicePointM>> GetServices();
        Task AddCustomerToQueue(ServicePointM customer);
        Task<IEnumerable<QueueM>> GetCalledCustomers();
        Task<IEnumerable<QueueM>> GetWaitingCustomers(string userServingPointId);
        Task<QueueM> MyCurrentServingCustomer(string userServingPointId);
        Task<QueueM> UpdateOutGoingAndIncomingCustomerStatus(int outgoingCustomerId, string serviceProviderId);
        Task<QueueM> GetCurentlyCalledNumber(string serviceProviderId);
        Task<QueueM> MarkNumberASNoShow(string serviceProviderId);
        Task<QueueM> MarkNumberASFinished(string serviceProviderId);
        Task<QueueM> TransferNumber(string serviceProviderId, int servicePointid);
    }
}

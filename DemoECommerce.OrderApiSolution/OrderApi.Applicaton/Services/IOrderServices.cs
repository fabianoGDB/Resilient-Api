using OrderApi.Applicaton.DTOs;

namespace OrderApi.Applicaton.Services
{
    public interface IOrderServices
    {
        Task<IEnumerable<OrderDTO>> GetOrdersByClientId(int clientId);
        Task<OrderDatailsDTO> GetOrderDetails(int orderId);
    }
}

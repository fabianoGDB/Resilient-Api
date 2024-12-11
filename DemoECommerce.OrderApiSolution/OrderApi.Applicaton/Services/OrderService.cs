using OrderApi.Applicaton.DTOs;
using OrderApi.Applicaton.DTOs.Conversions;
using OrderApi.Applicaton.Interfaces;
using Polly;
using Polly.Registry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Applicaton.Services
{
    public class OrderService(IOrder orderInterface, HttpClient httpClient, ResiliencePipelineProvider<string> resiliencePipeline) : IOrderServices
    {
        //GET PRODUCT
        public async Task<ProductDTO> GetProduct(int productId)
        {
            //Call the product api using HttpClient
            //redirect this call to the Api GateWay since product Api not response outsiders.
            var getProduct = await httpClient.GetAsync($"/api/products/{productId}");
            
            if (!getProduct.IsSuccessStatusCode)
            {

                return null!;
            }

            var product = await getProduct.Content.ReadFromJsonAsync<ProductDTO>();
            return product!;
        }

        //GET USER
        public async Task<AppUserDTO> GetUser(int userId) { 

            //Call the product api using HttpClient
            //redirect this call to the Api GateWay since product Api not response outsiders.
            var getUser = await httpClient.GetAsync($"api/authentication/{userId}");
            
            if (!getUser.IsSuccessStatusCode)
            {

                return null!;
            }

            //get Retry pipeline
            var user = await getUser.Content.ReadFromJsonAsync<AppUserDTO>();
            return user! ;

        }

        //Get Oorder Datails By Id
        public async Task<OrderDatailsDTO> GetOrderDetails(int orderId)
        {
            var order = await orderInterface.FindByIdAsync(orderId);
            if(order is null || order!.Id <= 0)
            {
                return null!;
            }

            
            var retryPipeline = resiliencePipeline.GetPipeline("my-retry-pipeline");

            var productDTO = await retryPipeline.ExecuteAsync(async token => await GetProduct(order.ProductId));

            var appUserDTO = await retryPipeline.ExecuteAsync(async token => await GetUser(order.ClientId));

            return new OrderDatailsDTO
                (
                order.Id,
                productDTO.Id,
                appUserDTO.Id,
                appUserDTO.Name,
                appUserDTO.Address,
                appUserDTO.Email,
                appUserDTO.TelephoneNumber,
                productDTO.Name,
                order.PurchaseQuantity,
                productDTO.Price,
                productDTO.Quantity * order.PurchaseQuantity,
                order.OrderedDate

                );
        }

        //GET ORDER BY CLIENT ID
        public async Task<IEnumerable<OrderDTO>> GetOrdersByClientId(int clientId)
        {
            //Get all client's orders
            var orders = await orderInterface.GetOrdersAsync(o => o.ClientId == clientId);
            if (!orders.Any())
            {
                return null!;
            }

            //Convert from entity to DTO
            var (_, _orders) = OrderConversion.FromEntity(null, orders);
            return _orders!;
        }
    }
}

using ECommerce.SharedLibrary.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Applicaton.DTOs;
using OrderApi.Applicaton.DTOs.Conversions;
using OrderApi.Applicaton.Interfaces;
using OrderApi.Applicaton.Services;

namespace OrderApi.Presentantion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController(IOrder orderInterface, IOrderServices orderServices) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrder()
        {
            var orders = await orderInterface.GetAllAsync();
            if (!orders.Any()) 
            {
                return NotFound();
            }

            var(_, list) = OrderConversion.FromEntity(null, orders);
            return !list!.Any() ?  NotFound() : Ok(list);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id) 
        {
            var order = await orderInterface.FindByIdAsync(id);
            if (order is null) 
            {
                NotFound(null);
            }

            var (_order, _) = OrderConversion.FromEntity(order, null);
            return Ok(_order);
        }

        [HttpGet("client/{clientId:int}")]
        public async Task<ActionResult<OrderDTO>> GetClientOrder(int clientId) 
        {
            if (clientId <= 0) { return BadRequest(); }

            var orders = await orderServices.GetOrdersByClientId(clientId);
            return !orders.Any() ? NotFound(null) : Ok(orders);
        }

        [HttpGet("details/{orderId:int}")]
        public async Task<ActionResult<OrderDatailsDTO>> GetOrderDetails(int orderId) 
        {
            if (orderId <= 0) { return BadRequest("Invalid data provided"); }

            var orderDetail = await orderServices.GetOrderDetails(orderId);
            return orderDetail.OrderId > 0 ? Ok(orderDetail) : NotFound(null); 
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateOrder(OrderDTO orderDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("This model is invalid");
            }

            var getEntity = OrderConversion.ToEntity(orderDTO);
            var response = await orderInterface.CreateAsync(getEntity);
            return response.Flag? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<ActionResult<Response>> UpdateOrder(OrderDTO orderDTO) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response(false, "This model is invalid"));
            }
            var order = OrderConversion.ToEntity(orderDTO);
            var response = await orderInterface.UpdateAsync(order);
            return response.Flag? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        public async Task<ActionResult<Response>> DeleteOrder(OrderDTO orderDTO) 
        {
            var order = OrderConversion.ToEntity(orderDTO);
            var response = await orderInterface.DeleteAsync(order);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VideoGames.Business.Abstract;
using VideoGames.Shared.ComplexTypes;
using VideoGames.Shared.DTOs;
using VideoGames.Shared.Helpers;

namespace VideoGames.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : CustomControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateDTO orderCreateDTO)
        {
            var response = await _orderService.CreateOrderAsync(orderCreateDTO);
            return CreateResponse(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _orderService.GetOrderAsync(id);
            return CreateResponse(response);

        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _orderService.GetOrdersAsync();
            return CreateResponse(response);    
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetByOrderStatus(OrderStatus orderStatus)
        {
            var response = await _orderService.GetOrdersAsync(orderStatus);
            return CreateResponse(response);
        }

        [HttpGet("getbyuserId/({applicationUserId})")]
        public async Task<IActionResult> GetByUser(string applicationUserId)
        {
            var response  = await _orderService.GetOrdersAsync(applicationUserId);    
            return CreateResponse(response);    
        }

        [HttpGet("daterange")]
        public async Task<IActionResult> GetByDate(DateTime startDate, DateTime endDate)
        {
            var response = await _orderService.GetOrdersAsync(startDate, endDate);
            return CreateResponse(response);

        }

        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, OrderStatus orderStatus)
        {
            var response = await _orderService.UpdateOrderStatusAsync(orderId, orderStatus);
            return CreateResponse(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var response = await _orderService.DeleteOrderAsync(id);
            return CreateResponse(response);
        }
    }
}

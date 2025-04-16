using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoGames.Business.Abstract;
using VideoGames.Shared.DTOs;
using VideoGames.Shared.Helpers;

namespace VideoGames.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : CustomControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize(Roles = "AdminUser")]
        [HttpGet("{applicationUserId}")]
        public async Task<IActionResult> GetCartByUser(string applicationUserId)
        {
            var response = await _cartService.GetCartAsync(applicationUserId);
            return CreateResponse(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart( CartCreateDTO cartCreateDTO)
        {
            var response = await _cartService.CreateCartAsync(cartCreateDTO);
            return CreateResponse(response);
        }

        [HttpPost("addtocart")]
        public async Task<IActionResult> AddToCart([FromBody] CartItemCreateDTO cartItemCreateDTO)
        {
            var response = await _cartService.AddVideoGameToCartAsync(cartItemCreateDTO);
            return CreateResponse(response);
        }

        [HttpDelete("item/{cartItemId}")]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            var response = await _cartService.RemoveVideoGameFromCartAsync(cartItemId);
            return CreateResponse(response);
        }


        [HttpDelete("clear/{applicationUserId}")]
        public async Task<IActionResult> ClearCart(string applicationUserId)
        {
            var response = await _cartService.ClearCartAsync(applicationUserId);
            return CreateResponse(response);
        }

        [HttpPut]
        public async Task<IActionResult> ChangeQuantity([FromBody] CartItemChangeQuantityDTO dto)
        {
            var response = await _cartService.ChangeVideoGameQuantityAsync(dto);
            return CreateResponse(response);
        }

    }
}

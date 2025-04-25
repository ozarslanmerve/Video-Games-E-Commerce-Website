using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Security.Claims;
using VideoGames.MVC.Abstract;
using VideoGames.MVC.Models;

namespace VideoGames.MVC.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IVideoGameService _videoGameService;
        private readonly IToastNotification _toaster;

        public CartController(ICartService cartService, IVideoGameService videoGameService, IToastNotification toaster)
        {
            _cartService = cartService;
            _videoGameService = videoGameService;
            _toaster = toaster;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _cartService.GetCartAsync(userId);

            if (cart == null)
            {
                _toaster.AddErrorToastMessage("Sepet bulunamadı.");
                return RedirectToAction("Index", "Home");
            }

            return View(cart);
        }

        public async Task<IActionResult> AddToCart(int videoGameId, int quantity)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["PendingVideoGameId"] = videoGameId;
                TempData["PendingQuantity"] = quantity;
                TempData["ReturnController"] = "Cart";
                TempData["ReturnAction"] = "AddToCart";
                _toaster.AddInfoToastMessage("Sepete eklemek için giriş yapmanız gerekmektedir.");
                return RedirectToAction("Login", "Auth");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var videoGame = await _videoGameService.GetAsync(videoGameId);
            if (videoGame == null)
            {
                return NotFound();
            }

            var cart = await _cartService.GetCartAsync(userId);
            if (cart == null)
            {
                await _cartService.CreateCartAsync(new CartModel
                {
                    ApplicationUserId = userId
                });
                cart = await _cartService.GetCartAsync(userId);
            }

            CartItemModel cartItem = new()
            {
                CartId = cart.Id,
                VideoGameId = videoGameId,
                Quantity = quantity
            };

            await _cartService.AddToCartAsync(cartItem);
            _toaster.AddSuccessToastMessage($"{quantity} adet {videoGame.Name} sepete eklendi.");
            return RedirectToAction("Index", "Home");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateQuantity([FromBody] CartItemModel model)
        {
            var result = await _cartService.ChangeQuantityAsync(model.Id, model.Quantity);
            if (string.IsNullOrWhiteSpace(result))
            {
                return Ok(); // Güncellendi
            }

            return BadRequest(result); // Hata mesajı varsa döndür
        }



        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            await _cartService.RemoveFromCartAsync(cartItemId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ClearCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _cartService.ClearCartAsync(userId);
            return RedirectToAction("Index");
        }
    }
}
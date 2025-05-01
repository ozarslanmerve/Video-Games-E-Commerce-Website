using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Security.Claims;
using VideoGames.MVC.Abstract;
using VideoGames.MVC.Models;

namespace VideoGames.MVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly IToastNotification _toaster;

        public OrderController(IOrderService orderService, ICartService cartService, IToastNotification toaster)
        {
            _orderService = orderService;
            _cartService = cartService;
            _toaster = toaster;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _orderService.GetByUserAsync(userId);
            return View(response);
        }

        [Authorize]
        public async Task<IActionResult> GetOrderByDateRange(DateTime startDate, DateTime endDate)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _orderService.GetByDateRangeAsync(startDate, endDate, userId);
            return View(response);

           
        }

        [Authorize]
        public async Task<IActionResult> CheckOut()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _cartService.GetCartAsync(userId);
            decimal totalPrice = cart.CartItems.Sum(x => x.VideoGame.Price * x.Quantity);
            return View(totalPrice);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Checkout(OrderCreateModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cart = await _cartService.GetCartAsync(userId);
            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
            {
                _toaster.AddErrorToastMessage("Sepetiniz boş.");
                return RedirectToAction("Index", "Cart");
            }

            // Sipariş verisi hazırlama
            model.ApplicationUserId = userId;
            model.OrderItems = cart.CartItems.Select(x => new OrderItemCreateModel
            {
                VideoGameId = x.VideoGame.Id,
                Quantity = x.Quantity
            }).ToList();

            // Siparişi API'ye gönder
            var result = await _orderService.CreateOrderAsync(model);

            if (result == null)
            {
                _toaster.AddErrorToastMessage("Sipariş oluşturulamadı. Yetersiz CD Key olabilir.");
                return RedirectToAction("Index", "Cart");
            }

            // Sepeti temizle
            await _cartService.ClearCartAsync(userId);

            _toaster.AddSuccessToastMessage("Sipariş başarıyla oluşturuldu!");
            return RedirectToAction("Index", "Order");
        }



        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetOrderAsync(id); // Tek sipariş getir
            if (order == null)
            {
                _toaster.AddErrorToastMessage("Sipariş bulunamadı.");
                return RedirectToAction(nameof(Index));
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (order.ApplicationUserId != currentUserId)
            {
                _toaster.AddErrorToastMessage("Bu siparişi görüntüleyemezsiniz.");
                return RedirectToAction(nameof(Index));
            }

            return View(order);
        }

    }
}

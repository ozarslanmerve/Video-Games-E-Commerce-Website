using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using VideoGames.MVC.Models;
using NToastNotify;
using VideoGames.MVC.Abstract;

namespace VideoGames.MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IToastNotification _toaster;
        private readonly ICartService _cartService;

        public AuthController(IAuthService authService, IToastNotification toaster, ICartService cartService)
        {
            _authService = authService;
            _toaster = toaster;
            _cartService = cartService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {

            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }
            try
            {
                var response = await _authService.LoginAsync(loginModel);
                if (response.Errors == null)
                {
                    var handler = new JwtSecurityTokenHandler();
                    var token = handler.ReadJwtToken(response.Data.AccessToken);

                    foreach (var claim in token.Claims)
                    {
                        Console.WriteLine($"Claim: {claim.Type} => {claim.Value}");
                    }
                    var userName =
                        token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ??
                        token.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

                    var userId =
                        token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                    var role =
                        token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                    if (userName != null)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Email, userName),
                            new Claim(ClaimTypes.Name, userName),
                            new Claim(ClaimTypes.NameIdentifier, userId),
                            new Claim(ClaimTypes.Role, role),
                            new Claim("AccessToken", response.Data.AccessToken)
                        };
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            principal,
                            new AuthenticationProperties
                            {
                                ExpiresUtc = response.Data.ExpirationDate,
                                IsPersistent = true
                            });
                        if (TempData["PendingVideoGameId"] != null && TempData["PendingQuantity"] != null)
                        {
                            string returnController = TempData["ReturnController"] as string ?? string.Empty;
                            string returnAction = TempData["ReturnAction"] as string ?? string.Empty;
                            int pendingVideoGameId = TempData["PendingVideoGameId"] as int? ?? 0;
                            int pendingQuantity = TempData["PendingQuantity"] as int? ?? 0;


                            var cart = await _cartService.GetCartAsync(userId);

                            if (cart != null)
                            {

                                await _cartService.AddToCartAsync(new CartItemModel
                                {
                                    CartId = cart.Id,
                                    VideoGameId = pendingVideoGameId,
                                    Quantity = pendingQuantity
                                });
                            }

                            return RedirectToAction(returnAction, returnController);
                        }
                    }

                    _toaster.AddSuccessToastMessage("Giriş İşlemi Başarıyla Tamamlandı");
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Giriş Hatası");
                return View(loginModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Giriş Hatası: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Bir hata oluştu, daha sonra tekrar deneyiniz");
                return View(loginModel);
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerModel);
            }

            try
            {
                var response = await _authService.RegisterAsync(registerModel);

                if (response.Errors == null || response.Errors.Count == 0)
                {
                    // Kullanıcı başarılı şekilde kaydolduysa:
                    // Kullanıcının email bilgisine göre tekrar login yaparak token al
                    var loginResponse = await _authService.LoginAsync(new LoginModel
                    {
                        Email = registerModel.Email,
                        Password = registerModel.Password
                    });

                    if (loginResponse?.Data?.AccessToken != null)
                    {
                        var handler = new JwtSecurityTokenHandler();
                        var token = handler.ReadJwtToken(loginResponse.Data.AccessToken);

                        var userId = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                        if (!string.IsNullOrEmpty(userId))
                        {
                            // Yeni kayıt olan kullanıcı için sepet oluştur
                            await _cartService.CreateCartAsync(new CartModel
                            {
                                ApplicationUserId = userId
                            });
                        }
                    }

                    _toaster.AddSuccessToastMessage("Kayıt işlemi başarıyla tamamlandı. Giriş yapabilirsiniz.");
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var error in response.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                    return View(registerModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kayıt Hatası: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Kayıt sırasında bir hata oluştu. Lütfen tekrar deneyiniz.");
                return View(registerModel);
            }
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _toaster.AddSuccessToastMessage("Çıkış işlemi başarıyla tamamlandı");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}

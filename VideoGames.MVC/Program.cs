using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using NToastNotify;
using VideoGames.MVC.Abstract;
using VideoGames.MVC.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllersWithViews()
    .AddNToastNotifyToastr(new ToastrOptions
    {
        ProgressBar = true, 
        PositionClass = ToastPositions.TopRight,
        CloseButton = true, 
        TimeOut = 5000, 
        ShowDuration = 1000, 
        HideDuration = 1000, 
        ShowEasing = "swing", 
        HideEasing = "linear", 
        ShowMethod = "fadeIn", 
        HideMethod = "fadeOut" 
    });

builder.Services.AddScoped<IVideoGameService, VideoGameService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICartService, CartService>();    
builder.Services.AddScoped<IOrderService, OrderService>();



builder
    .Services
    .AddHttpClient(
        "VideoGamesAPI",
        client => client.BaseAddress = new Uri("http://localhost:5178/api/")
    );

builder
    .Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "VideoGames.Auth";
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.Cookie.HttpOnly = true;
    });

builder.Services.AddAuthorization();

#region Data Protection
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(
        builder.Environment.ContentRootPath, "keys")))
    .SetApplicationName("VideoGames.MVC")
    .SetDefaultKeyLifetime(TimeSpan.FromDays(14));
builder.Services.AddDistributedMemoryCache();
#endregion

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseNToastNotify();

app.MapAreaControllerRoute(
    name: "admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}",
    defaults: new { area = "Admin" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();



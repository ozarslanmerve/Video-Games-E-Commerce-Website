using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllersWithViews()
    .AddNToastNotifyToastr(new ToastrOptions
    {
        ProgressBar = true, // ilerleme çubuðu
        PositionClass = ToastPositions.TopRight,// pozisyon
        CloseButton = true, // kapatma butonu
        TimeOut = 5000, // süresi 5 sn
        ShowDuration = 1000, // açýlýrken silikten görünür hale geçme süresi
        HideDuration = 1000, // kapanýrken silik hale geçme süresi
        ShowEasing = "swing", // açýlma efekti
        HideEasing = "linear", // 
        ShowMethod = "fadeIn", // görünür olma olayý
        HideMethod = "fadeOut" // kapanma olayý
    });


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
        options.Cookie.Name = "VideoGames.Authorization";
        options.LoginPath = "/Authorization/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied"; // yetkisiz sayfa
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.Cookie.HttpOnly = true; // sadece Http protokolü üzerine istek alsýn
    });

builder.Services.AddAuthorization();

#region Data Protection
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(
        builder.Environment.ContentRootPath, "keys")))
    .SetApplicationName("ECommerce.MVC")
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

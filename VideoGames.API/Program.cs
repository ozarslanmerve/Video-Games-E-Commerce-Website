using Microsoft.EntityFrameworkCore;
using VideoGames.Data.Abstract;
using VideoGames.Data.Concrete.Repositories;
using VideoGames.Data.Concrete;
using VideoGames.Data.Concrete.Contexts;
using VideoGames.Business.Abstract;
using VideoGames.Business.Concrete;
using VideoGames.Business.FileManagement.Abstract;
using VideoGames.Business.FileManagement.Concrete;
using VideoGames.Business;
using VideoGames.Entity.Concrete;
using Microsoft.AspNetCore.Identity;
using VideoGames.Business.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<VideoGamesDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IVideoGameService, VideoGameService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();

builder.Services.AddScoped<IFileService, FileService>();


builder.Services.AddAutoMapper(typeof(MappingProfile));



#region Identity Ayarlarý
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;

    //  (User) ile ilgili ayarlar:
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvyz0123456789-_.@";

})
    .AddEntityFrameworkStores<VideoGamesDbContext>()
    .AddDefaultTokenProviders();
#endregion


#region JWT Ayarlarý
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
JwtConfig jwtConfig = builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, 
        ValidateAudience = true, 
        ValidateLifetime = true, 
        ValidateIssuerSigningKey = true, 
        ValidIssuer = jwtConfig?.Issuer, 
        ValidAudience = jwtConfig?.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig?.Secret ?? "")),    
    };
});

#endregion


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using Microsoft.EntityFrameworkCore;
using VideoGames.Data.Abstract;
using VideoGames.Data.Concrete.Repositories;
using VideoGames.Data.Concrete;
using VideoGames.Data.Concrete.Contexts;
using VideoGames.Business.Abstract;
using VideoGames.Business.Concrete;
using VideoGames.Business.Configuration;
using VideoGames.Business.FileManagement.Abstract;
using VideoGames.Business.FileManagement.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<VideoGamesDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IVideoGameService, VideoGameService>();


builder.Services.AddScoped<IFileService, FileService>();


builder.Services.AddAutoMapper(typeof(MappingProfile));














var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

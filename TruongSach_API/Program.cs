using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TruongSach_API.Models;
using TruongSach_API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// lỗi gì lạ lắm
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});
builder.Services.AddControllers();
builder.Services.AddDbContext<TruongSachContext>(options =>options.UseSqlServer(builder.Configuration.GetConnectionString("TruongSach")));
builder.Services.AddScoped<INguoiDungRepository, NguoiDungRepository>();
builder.Services.AddScoped<IChiendichRepository, ChiendichRepository>();
builder.Services.AddScoped<IDongGopRepository, DongGopRepository>();
builder.Services.AddScoped<ISanphamRepository, SanphamRepository>();
builder.Services.AddScoped<ILoaisanphamRepository, LoaisanphamRepository>();
builder.Services.AddScoped<IHoaDonRepository, HoaDonRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:5278")
             .AllowAnyHeader()
             .AllowAnyMethod();
    });
});
var app = builder.Build();

app.UseCors("MyAllowOrigins");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

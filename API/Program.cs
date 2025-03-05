using Domain.Services;
using Data.Repository;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;
using Data.Interfaces;
using Data.Mapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registering the DbContext with connection string
builder.Services.AddDbContext<PedidosContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStrings:PedidosConnection")));

builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

// Registering AutoMapper with a specific profile
builder.Services.AddAutoMapper(typeof(DataMappingProfile));

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

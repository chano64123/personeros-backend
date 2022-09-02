using Microsoft.EntityFrameworkCore;
using Infraestructura.Data;
using Core.Model;
using Core.Interface;
using Infraestructura.Repository;
using API.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//para conectar a sql
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString, builder => builder.EnableRetryOnFailure()));

//para repositorio - aca se agregan todos
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

//servicio de automapper
builder.Services.AddAutoMapper(typeof(MappingProfiles));

//cors
builder.Services.AddCors();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();

using CricketApp.API.Common.MappingProfiles;
using CricketApp.API.Data;
using CricketApp.API.Helpers;
using CricketApp.API.Hubs;
using CricketApp.API.Interfaces;
using CricketApp.API.Repositories;
using CricketApp.API.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


SerilogConfig.ConfigureLogger(builder.Configuration);
builder.Host.UseSerilog();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200") 
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); 
    });
});


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddAutoMapper(typeof(PlayerProfile));
builder.Services.AddSignalR(); 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseCors("AllowAngular");

app.UseSerilogRequestLogging();
app.UseRouting();

app.UseAuthorization();
app.MapControllers();
app.MapHub<AppHub>("/hubs/app");

app.Run();

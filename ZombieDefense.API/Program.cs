

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ZombieDefense.API.Middleware;
using ZombieDefense.Application.Interfaces;
using ZombieDefense.Application.Services;
using ZombieDefense.Domain.Interfaces;
using ZombieDefense.Infrasctructure.Data;
using ZombieDefense.Infrasctructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IZombieRepository, ZombieRepository>();
builder.Services.AddScoped<ISimulacionRepository, SimulacionRepository>();
builder.Services.AddScoped<IZombieEliminadoRepository, ZombieEliminadoRepository>();

builder.Services.AddScoped<IDefenseStrategyService, DefenseStrategyService>();
builder.Services.AddScoped<ISimulacionService, SimulacionService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {

    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo {
        Title = "Zombie Horde Defense API",
        Version = "v1",
        Description = "API para calcular estrategias óptimas de defensa contra hordas de zombies.",
        Contact = new OpenApiContact {
            Name = "Tu Nombre",
            Email = "tu.correo@example.com"
        }
    });

    // Configurar para enviar API Key
    options.AddSecurityDefinition("X-API-KEY", new OpenApiSecurityScheme {
        Description = "Clave de acceso (X-API-KEY)",
        Name = "X-API-KEY",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "X-API-KEY"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "X-API-KEY"
                }
            },
            new List<string>()
        }
    });

});

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseMiddleware<ApiKeyMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();


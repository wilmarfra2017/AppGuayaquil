using AppGuayaquil.Api.Middleware;
using AppGuayaquil.Infrastructure.DataSource;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://*:8082");

var config = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Ingrese el token JWT",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<DataContext>(opts =>
{
    opts.UseSqlServer(config.GetConnectionString("db"));
});

builder.Services.AddScoped<AppGuayaquil.Domain.Ports.ICredentialsRepository, AppGuayaquil.Infrastructure.Adapters.CredentialsRepository>();
builder.Services.AddScoped<AppGuayaquil.Domain.Ports.IPeopleRepository, AppGuayaquil.Infrastructure.Adapters.PeopleRepository>();

builder.Services.AddMediatR(Assembly.Load("AppGuayaquil.Application"), typeof(Program).Assembly);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtConfig:Key"]!)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = config["JwtConfig:Issuer"],
            ValidAudience = config["JwtConfig:Audience"],
            ClockSkew = TimeSpan.Zero
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
        c.RoutePrefix = string.Empty; // Swagger en la raíz
    });
    app.UseDeveloperExceptionPage();
}

app.UseCustomExceptionHandler();

// Desactiva redirección HTTPS para pruebas locales
// app.UseHttpsRedirection();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

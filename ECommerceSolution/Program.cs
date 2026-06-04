using ECommerceSolution.Data;
using ECommerceSolution.Helpers;
using ECommerceSolution.Interfaces;
using ECommerceSolution.Middleware;
using ECommerceSolution.Repositories;
using ECommerceSolution.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ECommerceSolution.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration
        .GetConnectionString(
            "DefaultConnection"));
});

builder.Services.AddScoped<IUserRepository,
    UserRepository>();

builder.Services.AddScoped<IAuthService,
    AuthService>();

builder.Services.AddScoped<JwtHelper>();

builder.Services
.AddAuthentication(
JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer =
                builder.Configuration["Jwt:Issuer"],

            ValidAudience =
                builder.Configuration["Jwt:Audience"],

            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        builder.Configuration["Jwt:Key"]!))
        };
});

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}





app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();



using (var scope =
       app.Services.CreateScope())
{
    var context =
        scope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    await DbSeeder
        .SeedAdminAsync(context);
}

app.Run();


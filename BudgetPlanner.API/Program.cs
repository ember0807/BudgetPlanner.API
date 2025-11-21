// =========================================================================
// 1. ИМПОРТЫ (Imports)
// =========================================================================
using BudgetPlanner.API.Data;
using BudgetPlanner.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity; // Для Identity
using Microsoft.AspNetCore.Authentication.JwtBearer; // Для JWT
using Microsoft.IdentityModel.Tokens; // Для JWT
using System.Text; // Для кодирования ключа JWT
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// =========================================================================
// 2. РЕГИСТРАЦИЯ СЕРВИСОВ (Add Services)
// =========================================================================

// 2.1. Контекст БД (SQLite)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// 2.2. Сервисы Identity (Пользователи и Роли)
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    // Требования к паролю
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<AppDbContext>() // Используем AppDbContext для хранения Identity-данных
.AddDefaultTokenProviders();

// 2.3. Конфигурация JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// 2.4. Политика CORS (Для фронтенда React)
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// 2.5. Стандартные сервисы API
builder.Services.AddControllers();
builder.Services.AddOpenApi(); 

var app = builder.Build();

// =========================================================================
// 3. КОНВЕЙЕР ОБРАБОТКИ ЗАПРОСОВ (Middleware Pipeline)
// =========================================================================

// 3.1. OpenAPI/Swagger (только в режиме разработки)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// 3.2. CORS (ДОЛЖЕН БЫТЬ ПЕРЕД АУТЕНТИФИКАЦИЕЙ)
app.UseCors("CorsPolicy");

// 3.3. Authentication (Кто ты?)
app.UseAuthentication();

// 3.4. Authorization (Что тебе можно?)
app.UseAuthorization();

// 3.5. Маппинг контроллеров
app.MapControllers();

app.Run();

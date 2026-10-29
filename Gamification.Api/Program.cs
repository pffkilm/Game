using System.Text;
using Gamification.Api;
using Gamification.Api.Interfaces;
using Gamification.Api.Middleware; // Добавили для наших middleware
using Gamification.Api.Repositories;
using Gamification.Api.Services;
using Gamification.Core.Data; 
using Gamification.Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Добавляем контроллеры и Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 2. ПОДКЛЮЧАЕМ POSTGRESQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("GamificationDb")));

// 3. РЕГИСТРИРУЕМ СЕРВИСЫ
builder.Services.AddScoped<IPlayerRepository, EfCorePlayerRepository>();
builder.Services.AddSingleton<ILevelService, LevelService>();
builder.Services.AddSingleton<IEnumerable<AchievementDefinition>>(AchievementCatalog.GetAchievements());
builder.Services.AddScoped<IXpCalculator, XpCalculator>();
builder.Services.AddScoped<IAchievementEvaluator, AchievementEvaluator>();
builder.Services.AddScoped<GamificationEngine>();
builder.Services.AddScoped<JwtTokenService>(); // ДОБАВИЛИ: сервис токенов

// 4. НАСТРОЙКА JWT АУТЕНТИФИКАЦИИ
var jwtKey = builder.Configuration["Jwt:Key"]
             ?? throw new InvalidOperationException("Jwt:Key не задан в конфигурации.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "GamificationApi",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "GamificationClient",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });
builder.Services.AddScoped<DtoMappingService>();

// 5. НАСТРОЙКА SWAGGER (Добавляем кнопку Authorize)
// 5. НАСТРОЙКА SWAGGER (Добавляем кнопку Authorize)
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http, 
        Scheme = "bearer", // <--- пишем маленькими буквами
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Введите токен. Swagger сам добавит Bearer."
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});
var app = builder.Build();

// 6. НАСТРОЙКА HTTP КОНВЕЙЕРА (Middleware)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gamification API V1");
    });
}

app.UseHttpsRedirection();

// ВАЖНО: Порядок middleware строго обязателен!
app.UseMiddleware<ExceptionHandlingMiddleware>(); // 1. Ловим ошибки
app.UseMiddleware<RequestLoggingMiddleware>();    // 2. Логируем запросы

app.UseAuthentication(); // 3. Кто ты? (Проверка JWT)
app.UseAuthorization();  // 4. Что тебе можно? (Проверка [Authorize])

app.MapControllers();

app.Run();
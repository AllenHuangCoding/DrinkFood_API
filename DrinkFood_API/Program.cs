using CodeShare.Libs.GenericEntityFramework;
using DataBase;
using CodeShare.Libs.BaseProject;
using DrinkFood_API.Service;
using DrinkFood_API.Services;
using Microsoft.OpenApi.Models;
using DrinkFood_API.Utility;

var builder = WebApplication.CreateBuilder(args);

#region setting CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
    });

    options.AddPolicy("AllowOrigin", builder =>
    {
        builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
    });
});
#endregion

#region 紀錄log及驗證

builder.Services.AddMvc(
                config =>
                {
                    config.Filters.Add(typeof(HandleExceptionFilter));
                    config.Filters.Add(typeof(ApiLogResourceFilter));
                }
            //setting  System.Text.Json response to PascalCase
            ).AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

#endregion

// Add services to the container.

var environmentName = builder.Environment.EnvironmentName;
var configRoot = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
    .Build();

builder.Configuration.AddConfiguration(configRoot);

#region 增加的註冊服務

// 服務與工廠
builder.Services.AddScoped<DBContextFactory<EFContext>>();
builder.Services.AddScoped<TokenManager>();

builder.Services.AddScoped<IDmlLogService, LogService>();
builder.Services.AddScoped<IApiLogService, ApiLogService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// 自動注入繼承Base相關的類別
builder.Services.AddScopedByClassName("BaseTable");
builder.Services.AddScopedByClassName("BaseView");
builder.Services.AddScopedByClassName("BaseService");

builder.Services.AddHttpContextAccessor();

#endregion 

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "訂餐系統",
        Version = "v1.0",
        Description = "系統API規格說明"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Bearer Token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || environmentName == "Local")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowOrigin");

app.UseBaseMiddleware();

app.Run();

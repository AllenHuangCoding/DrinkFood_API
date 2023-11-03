using CodeShare.Libs.GenericEntityFramework;
using DataBase;
using DrinkFood_API.Service;
using Microsoft.Extensions.DependencyInjection;

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

// Add services to the container.

#region 增加的註冊服務

// 服務與工廠
builder.Services.AddSingleton<DBContextFactory<EFContext>>();

// 自動注入繼承Base相關的類別
builder.Services.AddScopedByClassName("BaseTable");
builder.Services.AddScopedByClassName("BaseView");
builder.Services.AddScopedByClassName("BaseService");
builder.Services.AddScopedByClassName("BaseController");

builder.Services.AddScoped<IDmlLogService, LogService>();

#endregion 

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseCors("AllowOrigin");

app.Run();

using System.Text.Json;
using WebPageService.Interfaces;
using WebPageService.Repositories;
using WebPageService.Services;

var builder = WebApplication.CreateBuilder(args);

var cfg = builder.Configuration;
var authBase = cfg["ExternalServices:AuthBaseUrl"] ?? "http://authservice:8080/api/auth/";
var storeBase = cfg["ExternalServices:StoreBaseUrl"] ?? "http://storeservice:8080/api/matches/";

// Controllers + JSON options
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// HttpClients
builder.Services.AddHttpClient<IAuthRepository, AuthRepository>(c =>
{
    c.BaseAddress = new Uri(authBase);
    c.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHttpClient<IStoreRepository, StoreRepository>(c =>
{
    c.BaseAddress = new Uri(storeBase);
    c.Timeout = TimeSpan.FromSeconds(30);
});

// Web service
builder.Services.AddScoped<IWebService, WebService>();

// CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173") // il tuo frontend React
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Applica CORS prima di Authorization e MapControllers
app.UseCors("AllowReact");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // disattivato per test locale
app.UseAuthorization();
app.MapControllers();
app.Run();
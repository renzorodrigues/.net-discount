using DiscountGRPCServer.Domain;
using DiscountGRPCServer.Repository;
using DiscountGRPCServer.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5116, o => o.Protocols = HttpProtocols.Http2);
});

builder.Services.AddGrpc();

var redisConfig = builder.Configuration.GetSection("Redis");
var redisHost = redisConfig["Host"] ?? "localhost";
var redisPort = redisConfig["Port"] ?? "6379";
var redisPassword = redisConfig["Password"] ?? "MyPassword";

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = $"{redisHost}:{redisPort},password={redisPassword}";
    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddSingleton<IDatabase>(sp =>
{
    var connection = sp.GetRequiredService<IConnectionMultiplexer>();
    return connection.GetDatabase();
});

builder.Services.AddSingleton<IDiscountRepository, RedisDiscountRepository>();
builder.Services.AddSingleton<IDiscountManager, DiscountManager>();
builder.Services.AddSingleton<DiscountServiceGrpc>();

var app = builder.Build();

app.MapGrpcService<DiscountServiceGrpc>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();

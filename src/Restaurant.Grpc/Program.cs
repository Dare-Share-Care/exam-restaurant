using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Restaurant.Core.Interfaces;
using Restaurant.Core.Services;
using Restaurant.Grpc.Services;
using Restaurant.Infrastructure.Data;
using Restaurant.Infrastructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);

//DBContext
builder.Services.AddDbContext<RestaurantContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbContext"));
});

//Build services
builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<ILoggingService, LoggingService>();

//Build repositories
builder.Services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

// Add services to the container.
builder.Services.AddGrpc();

builder.WebHost.ConfigureKestrel(options =>
{
    // Setup a HTTP/2 endpoint without TLS.
    options.ListenAnyIP(8000, o => o.Protocols = HttpProtocols.Http2);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<CatalogueService>();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
using Microsoft.EntityFrameworkCore;
using Restaurant.Web.Data;
using Restaurant.Web.Interface.DomainServices;
using Restaurant.Web.Interface.Repositories;
using Restaurant.Web.Migrations;
using Restaurant.Web.Service;


const string policyName = "AllowOrigin";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policyName,
        builder =>
        {
            builder
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});


//DBContext
builder.Services.AddDbContext<RestaurantContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbContext"));
});

//Build services
builder.Services.AddScoped<IRestaurantService, RestaurantService>();

//Build repositories
builder.Services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(policyName);
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
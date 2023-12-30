using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Restaurant.Core.Interfaces;
using Restaurant.Core.Services;
using Restaurant.Core.Types;
using Restaurant.Infrastructure.Data;
using Restaurant.Infrastructure.Interfaces;

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
                .AllowAnyOrigin() 
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
builder.Services.AddScoped<ILoggingService, LoggingService>();

//Build repositories
builder.Services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

//ILogger
builder.Services.AddLogging();

//JWT Key
// var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!);
var key = Encoding.UTF8.GetBytes("super_secret_key"); //TODO above doesn't work with docker.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
                        
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireCustomerRole", policy => policy.RequireRole("Customer"));
    options.AddPolicy("RequireRestaurantOwnerRole", policy => policy.RequireRole("RestaurantOwner"));
options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    // Add more policies for other roles as needed
});

var app = builder.Build();
app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(policyName);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
public partial class Program { }
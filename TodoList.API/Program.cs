using Microsoft.EntityFrameworkCore;
using TodoList.API.Middlewares;
using TodoList.Business.Mappings;
using TodoList.Business.Interfaces;
using TodoList.Business.Services;
using TodoList.DataAccess.Context;
using TodoList.DataAccess.Interfaces;
using TodoList.DataAccess.Repository;
using Serilog;
using Serilog.Formatting.Json;
using TodoList.API.Services.Interfaces;
using TodoList.API.Services;
using TodoList.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

Log.Information("Application Starting...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    //Serilog configuration:
    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(new JsonFormatter(), "logs/log-.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information);
    });

    // Identity configuration:
    builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>(x =>
    {
        x.Password.RequireDigit = false;
        x.Password.RequireLowercase = false;
        x.Password.RequireUppercase = false;
        x.Password.RequireNonAlphanumeric = false;
        x.Password.RequiredLength = 6;
    })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

    // JWT Authentication configuration:
    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
        .AddJwtBearer(j =>
        {
            j.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["JWTSettings:Issuer"],
                ValidAudience = builder.Configuration["JWTSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSettings:Secret"]!))
            };
        });

    // Add services to the container.
    builder.Services.AddDbContext<AppDbContext>(x =>
    {
        x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
    });

    //Unit of Work:
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

    //Repositories:
    builder.Services.AddScoped<ITodoItemService, TodoItemService>();
    builder.Services.AddScoped<ITodoListService, TodoListService>();
    builder.Services.AddScoped<IFileService, FileService>();

    builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseStaticFiles();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to starting.");
}
finally
{
    Log.CloseAndFlush();
}
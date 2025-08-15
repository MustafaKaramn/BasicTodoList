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

    // Add services to the container.
    builder.Services.AddDbContext<AppDbContext>(x =>
    {
        x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
    });

    //Unit of Work:
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

    //Repositories:
    builder.Services.AddScoped<ITodoService, TodoService>();


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
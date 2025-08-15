using Microsoft.EntityFrameworkCore;
using TodoList.API.Middlewares;
using TodoList.Business.Mappings;
using TodoList.Business.Interfaces;
using TodoList.Business.Services;
using TodoList.DataAccess.Context;
using TodoList.DataAccess.Interfaces;
using TodoList.DataAccess.Repository;

var builder = WebApplication.CreateBuilder(args);

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

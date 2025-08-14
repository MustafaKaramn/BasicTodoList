using Microsoft.EntityFrameworkCore;
using TodoList.API.Middlewares;
using TodoList.Business.Abstract;
using TodoList.Business.Concrete;
using TodoList.Business.Mappings;
using TodoList.DataAccess.Context;
using TodoList.DataAccess.Interfaces;
using TodoList.DataAccess.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

//Generic Repository:
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

//Services:
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

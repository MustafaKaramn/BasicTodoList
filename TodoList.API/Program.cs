using Microsoft.EntityFrameworkCore;
using TodoList.Business.Abstract;
using TodoList.Business.Concrete;
using TodoList.Business.Mappings;
using TodoList.DataAccess.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddScoped<ITodoRepository, TodoRepository>();



builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

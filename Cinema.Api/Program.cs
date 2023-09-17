using Cinema.Domain.Abstracts.RepositoryAbstracts;
using Cinema.Domain.Abstracts.UnitOfWorkAbstract;
using Cinema.Infrastructure.CinemaUnitOfWork;
using Cinema.Infrastructure.Data;
using Cinema.Infrastructure.Repositories.Base;
using CinemaApi.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddDbContext<CinemaContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
//});

builder.Services.AddControllers();

builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddScoped<ICinemaUnitOfWork, CinemaUnitOfWork>();

builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();

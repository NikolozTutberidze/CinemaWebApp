using Cinema.Application.ServiceImplementation;
using Cinema.Domain.Abstracts.RepositoryAbstracts;
using Cinema.Domain.Abstracts.ServiceAbstracts;
using Cinema.Infrastructure.Data;
using Cinema.Infrastructure.Repositories.Base;
using Cinema.Infrastructure.Repositories.OldRepos;
using CinemaApi.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<CinemaContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddControllers();

builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IMovieRepository, MovieRepositoryOld>()
                .AddScoped<IDirectorRepository, DirectorRepositoryOld>()
                .AddScoped<IGenreRepository, GenreRepositoryOld>()
                .AddScoped<IActorRepository, ActorRepositoryOld>();

builder.Services.AddScoped<IMovieService, MovieService>()
                .AddScoped<IDirectorService, DirectorService>()
                .AddScoped<IGenreService, GenreService>()
                .AddScoped<IActorService, ActorService>();

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

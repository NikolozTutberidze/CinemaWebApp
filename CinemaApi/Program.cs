using DataAccessLayer.Data;
using DataAccessLayer.Repositories.Implementation;
using DataAccessLayer.Repositories.Interfaces;
using LogicLayer.Services.Interfaces;
using LogicLayer.Services.Implementation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CinemaContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add services to the container
builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddScoped<IMovieService, MovieService>()
                .AddScoped<IDirectorService, DirectorService>()
                .AddScoped<IGenreService, GenreService>()
                .AddScoped<IActorService, ActorService>();

builder.Services.AddScoped<IMovieRepository, MovieRepository>()
                .AddScoped<IDirectorRepository, DirectorRepository>()
                .AddScoped<IGenreRepository, GenreRepository>()
                .AddScoped<IActorRepository, ActorRepository>();

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

app.MapControllers();

app.Run();

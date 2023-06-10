using AutoMapper;
using DataAccessLayer.Models;
using LogicLayer.Dtos;

namespace LogicLayer.Mappings
{
    internal class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddMovieDto, Movie>();
            CreateMap<ChangeMovieDto, Movie>();
            CreateMap<Movie, MovieDto>();

            CreateMap<AddGenreDto, Genre>();
            CreateMap<Genre, GenreDto>();
            CreateMap<ChangeGenreDto, Genre>();

            CreateMap<AddDirectorDto, Director>();
            CreateMap<Director, DirectorDto>();
            CreateMap<ChangeDirectorDto, Director>();
            CreateMap<Director, DirectorDtoForMovies>();

            CreateMap<AddActorDto, Actor>();
            CreateMap<Actor, ActorDto>();
            CreateMap<ChangeActorDto, Actor>();
        }
    }
}

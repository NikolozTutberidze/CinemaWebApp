using AutoMapper;
using Azure.Core;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interfaces;
using LogicLayer.Dtos;
using LogicLayer.Mappings;
using LogicLayer.Services.Interfaces;

namespace LogicLayer.Services.Implementation
{
    public class DirectorService : IDirectorService
    {
        private readonly IDirectorRepository _repository;
        private readonly IMapper _mapper;

        public DirectorService(IDirectorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());

            _mapper = new Mapper(config);
        }

        public async Task<DirectorDto> AddDirectorAsync(AddDirectorDto request)
        {
            if (request.FullName == null || request.FullName == string.Empty)
                throw new Exception("Input is incorrect");

            var director = _mapper.Map<Director>(request);
            await _repository.AddDirectorAsync(director);

            var directorDto = _mapper.Map<DirectorDto>(director);
            return directorDto;
        }

        public async Task ChangeDirectorAsync(ChangeDirectorDto request)
        {
            var director = _mapper.Map<Director>(request);

            await _repository.ChangeDirectorAsync(director);
        }

        public async Task DeleteDirectorAsync(Guid requsetId)
        {
            var director = await _repository.GetDirectorAsync(requsetId);

            if (director == null)
                throw new Exception("Director does not exist");

            await _repository.DeleteDirectorAsync(director);
        }

        public async Task<DirectorDto> GetDirectorAsync(Guid requestId)
        {
            var director = await _repository.GetDirectorAsync(requestId);

            if (director == null)
                throw new Exception("Director does not exist");

            var directorDto = _mapper.Map<DirectorDto>(director);

            List<MoviesForDirectorDto> movies = new List<MoviesForDirectorDto>();
            foreach (var item in director.Movies)
            {
                movies.Add(new MoviesForDirectorDto
                {
                    Id = item.Id,
                    Title = item.Title
                });
            }

            directorDto.Movies = movies;

            return directorDto;
        }

        public async Task<ICollection<DirectorDto>> GetDirectorsAsync()
        {
            var directors = await _repository.GetDirectorsAsync();

            if (directors.Count == 0)
                throw new Exception("Directors are empty");

            var directorsDto = directors.Select(d => _mapper.Map<DirectorDto>(d)).ToList();

            List<MoviesForDirectorDto> movies = new List<MoviesForDirectorDto>();

            for (int i = 0; i < directors.Count; ++i)
            {
                foreach (var item in directors.ElementAt(i).Movies)
                {
                    movies.Add(new MoviesForDirectorDto
                    {
                        Id = item.Id,
                        Title = item.Title
                    });
                }
                directorsDto.ElementAt(i).Movies = movies;
                movies.Clear();
            }

            return directorsDto;
        }
    }
}

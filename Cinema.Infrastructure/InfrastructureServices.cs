using Cinema.Domain.Abstracts.RepositoryAbstracts;
using Cinema.Domain.Abstracts.UnitOfWorkAbstract;
using Cinema.Infrastructure.Repositories.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Cinema.Infrastructure
{
    public static class InfrastructureServices
    {
        public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<ICinemaUnitOfWork, CinemaUnitOfWork.CinemaUnitOfWork>();

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }
    }
}

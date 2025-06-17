using Microsoft.Extensions.DependencyInjection;
using Domain.Repositories;
using Infrastructure.Repositories;
using Application.Abstractions.UnitOfWork;
using Domain.Common;
using Infrastructure.Common;

namespace Infrastructure
{
    /// <summary>
    /// Data Access Layer - Infrastructure Layer Dependency Injection
    /// Manages repositories, database context, and data-related services
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Register Data Access Layer services
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Updated service collection</returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Register repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }
    }
}
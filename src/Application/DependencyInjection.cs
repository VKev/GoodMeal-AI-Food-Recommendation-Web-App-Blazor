using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    /// <summary>
    /// Business Layer - Application Layer Dependency Injection
    /// Manages services, business logic, and application workflows
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Register Business Layer services
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Updated service collection</returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            // Register AutoMapper for object mapping
            services.AddAutoMapper(assembly);
            
            // Register Application Services
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
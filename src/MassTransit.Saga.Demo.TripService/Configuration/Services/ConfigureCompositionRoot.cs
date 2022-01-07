using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransit.Saga.Demo.TripService.Configuration.Services
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConfigureCompositionRoot
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCompositionRoot(this IServiceCollection services)
        {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddOptions();
            return services;
        }
    }
}

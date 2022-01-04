﻿using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCompositionRoot(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddOptions();
            return services;
        }
    }
}
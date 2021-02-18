﻿using AutoMapper;
using Products.Domain.DataAccess.S3;
using Products.Infraestructure.DataAccess.S3;
using Products.Infraestructure.Logging;
using Products.Infraestructure.Security;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;
using System;

namespace Products.Application.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            IConfigurationRoot configuration = GetConfiguration();
            services.AddSingleton<IConfiguration>(configuration);

#if DEBUG
            services.AddDefaultAWSOptions(configuration.GetAWSOptions());
#endif
            services.AddMediatR(AppDomain.CurrentDomain.Load("Products.Application"));
            services.AddAutoMapper(typeof(Function).Assembly);
            services.AddSingleton<ILogger, Logger>();
            services.AddSingleton<IAwsSecretManagerService, AwsSecretManagerService>();

            services.AddTransient<IProductsRepository, ProductsRepository>();

            return services;
        }

        private static IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile($"appsettings.json")
                            .AddEnvironmentVariables();

            var configuration = builder.Build();
            return configuration;
        }
    }
}
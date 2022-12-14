using Core.Utilities.Security.JWT;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddCoreService(this IServiceCollection services)
        {
            services.AddScoped<ITokenHandler, TokenHandler>();
            return services;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Concrete;

namespace Business
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddBusinessService(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IWriterService, WriterService>();
            services.AddTransient<ICategoryService, CategoryService>();

            return services;
        }
    }
}

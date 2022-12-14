using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Concrete;
using FluentValidation.AspNetCore;

namespace Business
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddBusinessService(this IServiceCollection services)
        {
            services.AddFluentValidation(options =>
            {
                // Validate child properties and root collection elements
                options.ImplicitlyValidateChildProperties = true;
                options.ImplicitlyValidateRootCollectionElements = true;

                // Automatic registration of validators in assembly
                options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            });
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IWriterService, WriterService>();
            services.AddTransient<ICategoryService, CategoryService>();

            return services;
        }
    }
}

using DataAccess.Concrete.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddDataAccessService(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<BlogDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgreSQL")));

            services.AddTransient<IUserDal, UserDal>();
            services.AddTransient<IRoleDal, RoleDal>();
            services.AddTransient<IWriterDal, WriterDal>();
            services.AddTransient<ICategoryDal, CategoryDal>();
            services.AddTransient<IBlogDal, BlogDal>();
            return services;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Utilities.Security.Hashing;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Concrete.Context
{
    public class BlogDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public BlogDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("PostgreSQL"));

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var date = new DateOnly(2022, 1, 1);
            HashingHelper.CreatePasswordHash("sau", out var hash, out var salt);

            List<Role> roles = new()
            {
                new() { Id = 1, Name = "Admin", Status = true, CreatedDate = date },
                new() { Id = 2, Name = "User", Status = true, CreatedDate = date},
            }; modelBuilder.Entity<Role>().HasData(roles);
            List<Category> categories = new()
            {
                new() { Id = 1, Name = "Technology", Status = true, CreatedDate = date },
                new() { Id = 2, Name = "Travel", Status = true, CreatedDate = date },
                new() { Id = 3, Name = "Personal", Status = true, CreatedDate = date },
                new() { Id = 4, Name = "Music", Status = true, CreatedDate = date },
                new() { Id = 5, Name = "Food", Status = true, CreatedDate = date },
                new() { Id = 6, Name = "Political", Status = true, CreatedDate = date },
                new() { Id = 7, Name = "News", Status = true, CreatedDate = date },
                new() { Id = 8, Name = "Lifestyle", Status = true, CreatedDate = date },
                new() { Id = 9, Name = "Fashion", Status = true, CreatedDate = date },
            }; modelBuilder.Entity<Category>().HasData(categories);
           
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var datas = ChangeTracker.Entries<BaseEntity>();
            foreach (var data in datas)
            {
                _ = data.State switch
                {
                    EntityState.Added => data.Entity.CreatedDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    EntityState.Modified => data.Entity.UpdatedDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    _ => DateOnly.FromDateTime(DateTime.UtcNow),
                };
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
        public DbSet<Writer> Writers { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}

using Domain.Common;
using Domain.Entities;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Domain.Entities.Task> Tasks { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            var baseEntityType = typeof(BaseEntity);
            var dbSetProperties = GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType.IsGenericType &&
                            p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                            baseEntityType.IsAssignableFrom(p.PropertyType.GetGenericArguments()[0]));

            foreach (var dbSetProperty in dbSetProperties)
            {
                var entityType = dbSetProperty.PropertyType.GetGenericArguments()[0];

                // Build HasQueryFilter expression: e => !e.IsDeleted
                var parameter = Expression.Parameter(entityType, "e");
                var isDeletedProperty = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                var notDeleted = Expression.IsFalse(isDeletedProperty);
                var lambda = Expression.Lambda(notDeleted, parameter);

                // Apply HasQueryFilter
                modelBuilder.Entity(entityType).HasQueryFilter(lambda);

                // Apply RowVersion
                modelBuilder.Entity(entityType).Property(nameof(BaseEntity.RowVersion)).IsRowVersion();
            }

            modelBuilder.Ignore<BaseEntity>();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                    entry.Entity.CreatedBy = null; // TODO: Add current user
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifiedOn = DateTime.UtcNow;
                    entry.Entity.ModifiedBy = null; // TODO: Add current user
                }

                if (entry.Entity.IsDeleted)
                {
                    entry.State = EntityState.Unchanged;
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}

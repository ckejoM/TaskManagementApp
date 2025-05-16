using Application.Contracts;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        private readonly ICurrentUserService _currentUserService;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Domain.Entities.Task> Tasks { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {          
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.IsAssignableTo(typeof(BaseEntity)))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var body = Expression.Not(
                        Expression.Property(parameter, nameof(BaseEntity.IsDeleted)));
                    var lambda = Expression.Lambda(body, parameter);

                    entityType.SetQueryFilter(lambda);
                }

                var rowVersionProperty = entityType.FindProperty(nameof(BaseEntity.RowVersion));
                if (rowVersionProperty != null)
                {
                    rowVersionProperty.ValueGenerated = ValueGenerated.OnAddOrUpdate;
                    rowVersionProperty.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
                    rowVersionProperty.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
                }
            }

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                    entry.Entity.CreatedBy = _currentUserService.GetCurrentUserId();
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifiedOn = DateTime.UtcNow;
                    entry.Entity.ModifiedBy = _currentUserService.GetCurrentUserId();
                }

                if (entry.Entity.IsDeleted)
                {
                    entry.State = EntityState.Unchanged;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebEnterprise.Models.Common;
using WebEnterprise.Models.Entities;

namespace WebEnterprise.Infrastructure.Persistance
{
    public abstract class AuditableDbContext : IdentityDbContext<User>
    {
        public AuditableDbContext(DbContextOptions options) : base(options)
        {
        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    //modelBuilder.Entity<User>(
        //    //  users =>
        //    //  {

        //    //      users.ToTable("Users").Property(p => p.Id).HasColumnName("UserId");
        //    //  });

        //}

        public virtual async Task<int> SaveChangesAsync(string username = "SYSTEM")
        {
            foreach (var entry in base.ChangeTracker.Entries<EntityBase>()
                .Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
            {
                entry.Entity.LastModifiedDate = DateTime.Now;
                entry.Entity.LastModifiedBy = username;

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = DateTime.Now;
                    entry.Entity.CreatedBy = username;
                }
            }

            var result = await base.SaveChangesAsync();

            return result;
        }
    }
}

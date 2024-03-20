using Microsoft.EntityFrameworkCore;
using WebEnterprise.Models.Entities;

namespace WebEnterprise.Infrastructure.Persistance
{
    public class UniversityDbContext : AuditableDbContext
    {
        public UniversityDbContext(DbContextOptions<UniversityDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(UniversityDbContext).Assembly);
            base.OnModelCreating(builder);

            builder.Entity<User>(
               users =>
               {

                   users.ToTable("Users").Property(p => p.Id).HasColumnName("UserId");
               });

            builder.Entity<Faculty>().HasData(
                new Faculty { Id = 1, Name = "Computing and Information Technology" },
                new Faculty { Id = 2, Name = "Business" }
                );
        }

        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Contribution> Contributions { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Megazine> Megazines { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
    }
}

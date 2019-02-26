using Microsoft.EntityFrameworkCore;
using Skeleton.ServiceName.Data.Configuration;

namespace Skeleton.ServiceName.Data
{
    public class ServiceNameContext : DbContext
    {
        public DbSet<Person> People { get; set; }

        public ServiceNameContext(DbContextOptions<ServiceNameContext> options)
            : base(options)
        {
            //irá criar o banco e a estrutura de tabelas necessárias
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("Server=localhost;User Id=pecege;Password=p3c3g3esalq;Database=skeleton");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration<Person>(new PersonConfiguration());
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Skeleton.ServiceName.Data.Configuration;
using Skeleton.ServiceName.Data.Models;
using Skeleton.ServiceName.Utils.Helpers;
using Skeleton.ServiceName.Utils.Security;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Skeleton.ServiceName.Data
{
    public class ServiceNameContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DbSet<User> Users { get; set; }
        public DbSet<Person> People { get; set; }

        public ServiceNameContext(DbContextOptions<ServiceNameContext> options, IHttpContextAccessor httpContextAccessor)
           : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
        }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTimestamps();
            return await base.SaveChangesAsync();
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            var userId = !string.IsNullOrEmpty(_httpContextAccessor.HttpContext?.User?.Identity?.Name) ?
                            JsonConvert.DeserializeObject<SecurityUserModel>(_httpContextAccessor.HttpContext.User.Identity.Name).Id.ToString() :
                            "Anonymous";

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).DateCreated = DateTimeHelper.BrazilNow;
                    ((BaseEntity)entity.Entity).UserCreated = userId;
                }

                ((BaseEntity)entity.Entity).DateModified = DateTimeHelper.BrazilNow;
                ((BaseEntity)entity.Entity).UserModified = userId;
            }
        }
    }
}

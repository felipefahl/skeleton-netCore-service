using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skeleton.ServiceName.Data.Models;

namespace Skeleton.ServiceName.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.UserCreated)
                .HasMaxLength(255);

            builder.Property(c => c.UserModified)
                .HasMaxLength(255);


            builder.Property(c => c.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(c => c.Email)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(c => c.Password)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(c => c.PasswordCheck)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(c => c.Profile)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(c => c.Active)
                .IsRequired()
                .HasDefaultValue(true);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skeleton.ServiceName.Data.Models;

namespace Skeleton.ServiceName.Data.Configuration
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("People");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(c => c.LastName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(c => c.BirthDate)
                .IsRequired();

            builder.Property(c => c.UserCreated)
                .HasMaxLength(255);

            builder.Property(c => c.UserModified)
                .HasMaxLength(255);

            builder.Property(c => c.Active)
                .IsRequired()
                .HasDefaultValue(true);
        }
    }
}

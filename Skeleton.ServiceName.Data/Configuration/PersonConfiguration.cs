using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Skeleton.ServiceName.Data.Configuration
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("People");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.FirstName)
                .HasColumnType("nvarchar(100)")
                .IsRequired();

            builder.Property(c => c.LastName)
                .HasColumnType("nvarchar(100)")
                .IsRequired();

            builder.Property(c => c.BirthDate)
                .IsRequired();
        }
    }
}

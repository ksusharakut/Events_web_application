using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Data.EntityConfigurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.Description)
               .HasMaxLength(1000)
               .IsRequired(false);

            builder.Property(e => e.DateTime)
                .IsRequired();

            builder.Property(e => e.Location)
                .IsRequired();

            builder.Property(e => e.Category)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.MaxParticipants)
                .IsRequired();

            builder.Property(e => e.ImageUrl)
                .IsRequired(false);
        }
    }
}

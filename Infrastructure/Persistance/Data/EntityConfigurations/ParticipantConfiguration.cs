using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Data.EntityConfigurations
{
    public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
    {
        public void Configure(EntityTypeBuilder<Participant> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Email)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(p => p.BirthDay)
                .IsRequired();

            builder.Property(p => p.PasswordHash)
                .IsRequired();
        }
    }
}

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Data.EntityConfigurations
{
    public class ParticipantEventConfiguration : IEntityTypeConfiguration<ParticipantEvent>
    {
        public void Configure(EntityTypeBuilder<ParticipantEvent> builder)
        {
            builder.HasKey(pe => new { pe.ParticipantId, pe.EventId });

            builder.Property(pe => pe.RegistrationDateTime)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(pe => pe.Participant)
                .WithMany(p => p.Events)
                .HasForeignKey(pe => pe.ParticipantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pe => pe.Event)
                .WithMany(e => e.Participants)
                .HasForeignKey(pe => pe.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

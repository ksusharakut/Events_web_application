using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Persistance.Data
{
    public sealed class ApplicationDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<ParticipantEvent> ParticipantEvents { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}

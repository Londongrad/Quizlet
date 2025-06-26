using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizlet.Domain.Entities;

namespace Quizlet.Infrastructure.Data.Configurations
{
    public class SetConfiguration : IEntityTypeConfiguration<Set>
    {
        public void Configure(EntityTypeBuilder<Set> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Description)
                .HasMaxLength(500);

            builder.Property(s => s.CreatedAt)
                .IsRequired();

            builder.HasOne(s => s.User)
                .WithMany(u => u.Sets)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.Words)
                .WithOne(w => w.Set)
                .HasForeignKey(w => w.SetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

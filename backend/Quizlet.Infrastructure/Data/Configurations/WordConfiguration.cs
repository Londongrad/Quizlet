using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Quizlet.Domain.Entities;

namespace Quizlet.Infrastructure.Data.Configurations
{
    public class WordConfiguration : IEntityTypeConfiguration<Word>
    {
        public void Configure(EntityTypeBuilder<Word> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(w => w.Definition)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(c => c.ImageURL)
                .HasMaxLength(300);

            builder.Property(w => w.CreatedAt)
                .IsRequired();

            builder.HasOne(w => w.Set)
                .WithMany(s => s.Words)
                .HasForeignKey(w => w.SetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

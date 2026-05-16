using Fanpa.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fanpa.Persistence.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<ReviewEntity>
{
    public void Configure(EntityTypeBuilder<ReviewEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Theme)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(2056);

        builder.Property(x => x.Money)
            .IsRequired();

        builder.Property(x => x.Rating)
            .IsRequired()
            .HasMaxLength(5);
        
        builder.Property(x => x.GameName)
            .IsRequired()
            .HasMaxLength(128);

        // скорее всего не нужно, но как подсказку, себе закоменчу
        // ведь отзыв не появлеятся сразу с ответом от селлера
        
        //builder.Property(x => x.SellerAnswer)
        //    .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

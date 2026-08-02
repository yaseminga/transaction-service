using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Entities;

namespace TransactionService.Infrastructure.Data.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                   .IsRequired()
                   .HasMaxLength(36)
                   .IsUnicode(false);

            builder.Property(x => x.Amount)
                   .IsRequired()
                   .HasPrecision(18, 2);

            builder.Property(x => x.Type)
                   .IsRequired();

            builder.Property(x => x.CreatedAt)
                   .IsRequired();

            builder.HasOne(x => x.User)
                   .WithMany(x => x.Transactions)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.UserId);

            builder.HasIndex(x => x.Type);

            builder.HasIndex(x => x.Amount);
        }
    }
}

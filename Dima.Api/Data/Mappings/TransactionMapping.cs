using Dima.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dima.Api.Data.Mappings;

public class TransactionMapping : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transaction");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasColumnType("NVARCHAR(80)");

        builder.Property(x => x.Type)
            .IsRequired()
            .HasColumnType("TINYINT");

        builder.Property(x => x.Amount)
            .IsRequired()
            .HasColumnType("MONEY");

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.PaidOrReceivedAt)
            .IsRequired(false);

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasColumnType("NVARCHAR(160)"); ;
    }

}
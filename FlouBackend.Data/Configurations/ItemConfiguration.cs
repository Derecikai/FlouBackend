using FlouBackend.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlouBackend.Data.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.Property(i => i.Name).IsRequired().HasMaxLength(200);

        builder.HasOne(i => i.Folder)
            .WithMany(f => f.Items)
            .HasForeignKey(i => i.FolderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.User)
            .WithMany()
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(i => i.UserId);
    }
}

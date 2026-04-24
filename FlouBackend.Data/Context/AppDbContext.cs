using FlouBackend.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlouBackend.Data.Context;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // ── Business tables ────────────────────────────────────────────────────────
    public DbSet<ActionType> ActionTypes => Set<ActionType>();
    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();
    public DbSet<CodeDetail> CodeDetails => Set<CodeDetail>();
    public DbSet<EntityType> EntityTypes => Set<EntityType>();
    public DbSet<Folder> Folders => Set<Folder>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<ItemTag> ItemTags => Set<ItemTag>();
    public DbSet<ItemType> ItemTypes => Set<ItemType>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<UrlDetail> UrlDetails => Set<UrlDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Identity tables (AspNetUsers etc.) — must come first
        base.OnModelCreating(modelBuilder);

        // ── Business table configurations (scaffolded) ─────────────────────────
        modelBuilder.Entity<ActionType>(entity =>
        {
            entity.HasIndex(e => e.Code, "UQ_ActionTypes_Code").IsUnique();
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);

            // Seed: these rows must always exist — referenced by ActivityLogs
            entity.HasData(
                new ActionType { Id = 1, Code = "created",    Name = "Created"    },
                new ActionType { Id = 2, Code = "updated",    Name = "Updated"    },
                new ActionType { Id = 3, Code = "deleted",    Name = "Deleted"    },
                new ActionType { Id = 4, Code = "restored",   Name = "Restored"   },
                new ActionType { Id = 5, Code = "moved",      Name = "Moved"      },
                new ActionType { Id = 6, Code = "archived",   Name = "Archived"   },
                new ActionType { Id = 7, Code = "unarchived", Name = "Unarchived" },
                new ActionType { Id = 8, Code = "tagged",     Name = "Tagged"     },
                new ActionType { Id = 9, Code = "untagged",   Name = "Untagged"   }
            );
        });

        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.HasIndex(e => e.CreatedAt, "ix_ActivityLogs_CreatedAt");
            entity.HasIndex(e => new { e.EntityTypeId, e.EntityId }, "ix_ActivityLogs_Entity");
            entity.HasIndex(e => new { e.UserId, e.CreatedAt }, "ix_ActivityLogs_UserId_CreatedAt").IsDescending(false, true);

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.EntityId).HasMaxLength(50);
            entity.Property(e => e.EntityName).HasMaxLength(300);
            entity.Property(e => e.IpAddress).HasMaxLength(45);

            entity.HasOne(d => d.ActionType).WithMany(p => p.ActivityLogs)
                .HasForeignKey(d => d.ActionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActivityLogs_ActionType");

            entity.HasOne(d => d.EntityType).WithMany(p => p.ActivityLogs)
                .HasForeignKey(d => d.EntityTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActivityLogs_EntityType");
        });

        modelBuilder.Entity<CodeDetail>(entity =>
        {
            entity.HasKey(e => e.ItemId);
            entity.Property(e => e.ItemId).ValueGeneratedNever();
            entity.Property(e => e.Language).HasMaxLength(50);

            entity.HasOne(d => d.Item).WithOne(p => p.CodeDetail)
                .HasForeignKey<CodeDetail>(d => d.ItemId)
                .HasConstraintName("FK_CodeDetails_Item");
        });

        modelBuilder.Entity<EntityType>(entity =>
        {
            entity.HasIndex(e => e.Code, "UQ_EntityTypes_Code").IsUnique();
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);

            // Seed: describes what kind of entity was acted on in ActivityLogs
            entity.HasData(
                new EntityType { Id = 1, Code = "item",   Name = "Item"   },
                new EntityType { Id = 2, Code = "folder", Name = "Folder" },
                new EntityType { Id = 3, Code = "tag",    Name = "Tag"    }
            );
        });

        modelBuilder.Entity<Folder>(entity =>
        {
            entity.HasIndex(e => new { e.UserId, e.ParentFolderId }, "ix_Folders_UserId_Parent");
            entity.HasIndex(e => new { e.UserId, e.ParentFolderId, e.Name }, "uq_Folders_UserId_Parent_Name")
                .IsUnique()
                .HasFilter("([IsDeleted]=(0))");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.ParentFolder).WithMany(p => p.InverseParentFolder)
                .HasForeignKey(d => d.ParentFolderId)
                .HasConstraintName("FK_Folders_Parent");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasIndex(e => new { e.UserId, e.CreatedAt }, "ix_Items_UserId_CreatedAt").IsDescending(false, true);
            entity.HasIndex(e => new { e.UserId, e.FolderId }, "ix_Items_UserId_FolderId");
            entity.HasIndex(e => new { e.UserId, e.ItemTypeId }, "ix_Items_UserId_TypeId");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Title).HasMaxLength(300);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Folder).WithMany(p => p.Items)
                .HasForeignKey(d => d.FolderId)
                .HasConstraintName("FK_Items_Folder");

            entity.HasOne(d => d.ItemType).WithMany(p => p.Items)
                .HasForeignKey(d => d.ItemTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Items_ItemType");
        });

        modelBuilder.Entity<ItemTag>(entity =>
        {
            entity.HasKey(e => new { e.ItemId, e.TagId });
            entity.HasIndex(e => e.TagId, "ix_ItemTags_TagId");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Item).WithMany(p => p.ItemTags)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_ItemTags_Item");

            entity.HasOne(d => d.Tag).WithMany(p => p.ItemTags)
                .HasForeignKey(d => d.TagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItemTags_Tag");
        });

        modelBuilder.Entity<ItemType>(entity =>
        {
            entity.HasIndex(e => e.Code, "UQ_ItemTypes_Code").IsUnique();
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.DisplayName).HasMaxLength(100);
            entity.Property(e => e.IconName).HasMaxLength(50);

            // Seed: defines what type an Item is (note, url, code snippet)
            entity.HasData(
                new ItemType { Id = 1, Code = "note", DisplayName = "Note",         IconName = "file-text" },
                new ItemType { Id = 2, Code = "url",  DisplayName = "Link",         IconName = "link"      },
                new ItemType { Id = 3, Code = "code", DisplayName = "Code Snippet", IconName = "code"      }
            );
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasIndex(e => new { e.UserId, e.Name }, "UQ_Tags_UserId_Name").IsUnique();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<UrlDetail>(entity =>
        {
            entity.HasKey(e => e.ItemId);
            entity.HasIndex(e => e.Domain, "ix_UrlDetails_Domain");

            entity.Property(e => e.ItemId).ValueGeneratedNever();
            entity.Property(e => e.Domain).HasMaxLength(255);
            entity.Property(e => e.ThumbnailUrl).HasMaxLength(2000);
            entity.Property(e => e.Url).HasMaxLength(2000);

            entity.HasOne(d => d.Item).WithOne(p => p.UrlDetail)
                .HasForeignKey<UrlDetail>(d => d.ItemId)
                .HasConstraintName("FK_UrlDetails_Item");
        });

        // ── Foreign keys to AspNetUsers (not auto-scaffolded) ──────────────────
        modelBuilder.Entity<Folder>()
            .HasOne<IdentityUser>()
            .WithMany()
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Item>()
            .HasOne<IdentityUser>()
            .WithMany()
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Tag>()
            .HasOne<IdentityUser>()
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

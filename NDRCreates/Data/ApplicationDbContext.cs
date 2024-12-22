using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using NDRCreates.Models.Entities;

using System.Reflection.Emit;

namespace NDRCreates.Data
{
    public class ApplicationDbContext : IdentityDbContext<BasicUser, BasicRole, string, BasicUserClaim, BasicUserRole, BasicUserLogin, BasicRoleClaim, BasicUserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<UnityPackage> UnityPackage { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BasicUser>(b =>
            {
                // Each User can have many UserClaims
                b.HasMany(e => e.Claims)
                    .WithOne(e => e.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.Logins)
                    .WithOne(e => e.User)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.Tokens)
                    .WithOne(e => e.User)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.Role)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<BasicRole>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                // Each Role can have many associated RoleClaims
                b.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();
            });

            // Configure the relationship between BlogPost and Comment
            modelBuilder.Entity<BlogPost>()
                .HasMany(b => b.Comments)
                .WithOne(c => c.BlogPost)
                .HasForeignKey(c => c.BlogPostId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the relationship between Comment and BasicUser
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Author)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Change the delete behavior to Restrict


            modelBuilder.Entity<BlogPost>()
                .HasOne(b => b.Author)
                .WithMany(u => u.BlogPosts)
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<BlogPost>()
                .HasMany(b => b.Tags)
                .WithMany(t => t.BlogPosts);

            modelBuilder.Entity<BlogPost>()
                .HasOne(b => b.Category)
                .WithMany(c => c.BlogPosts)
                .HasForeignKey(b => b.CategoryId);

            modelBuilder.Entity<BlogPost>()
                .HasMany(b => b.Likes)
                .WithOne(l => l.BlogPost)
                .HasForeignKey(l => l.BlogPostId);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

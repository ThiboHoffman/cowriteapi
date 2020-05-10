using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;
using coWriteAPI.Model;

namespace coWriteApi.Data
{
    public class ApplicationContext : IdentityDbContext
    {

        public DbSet<Author> Authors { get; set; }
        public DbSet<Story> Stories { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Author>().HasKey(u => u.AuthorId);
            builder.Entity<Author>().Property(u => u.Nickname).IsRequired().HasMaxLength(30);
            builder.Entity<Author>().Property(c => c.Email).IsRequired().HasMaxLength(100);
            builder.Entity<Author>().Ignore(c => c.FavoriteStories);
            builder.Entity<Author>().HasMany(a => a.Stories);


            builder.Entity<Story>().HasMany(s => s.Chapters)
                .WithOne()
                .IsRequired()
                .HasForeignKey("StoryId"); //Shadow property
            builder.Entity<Story>().Property(r => r.Name).IsRequired().HasMaxLength(50);
            //builder.Entity<Story>().HasMany(s => s.Authors);
            builder.Entity<Story>().Property(s => s.Favorites).IsRequired();

            builder.Entity<Chapter>().Property(r => r.Name).IsRequired().HasMaxLength(50);
            builder.Entity<Chapter>().Property(r => r.Text).HasMaxLength(10000);
            //builder.Entity<Chapter>().Property(c => c.Number).IsRequired();
            builder.Entity<Chapter>().Property(c => c.Author).IsRequired().HasMaxLength(30);

            builder.Entity<AuthorFavorite>().HasKey(f => new { f.UserId, f.StoryId });
            builder.Entity<AuthorFavorite>().HasOne(f => f.User).WithMany(u => u.Favorites).HasForeignKey(f => f.UserId);
            builder.Entity<AuthorFavorite>().HasOne(f => f.Story).WithMany().HasForeignKey(f => f.StoryId);
        }
        
    }
}
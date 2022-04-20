using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using template_csharp_blog.Models;

namespace template_csharp_blog
{
    public class ApplicationContext : DbContext
    {

        const string CONNECTION_STRING = "Server=(localdb)\\mssqllocaldb; Database=BlogDb; Trusted_Connection=True";

        //DbSet<User> Users { get; set; } //user -> blog one-to-one relationship
        //DbSet<Blog> Blogs { get; set; } //blog -> posts one-to-many relationship

        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostTag> PostTags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(CONNECTION_STRING);
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostTag>().HasKey(pt => new { pt.PostId, pt.TagId });

            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Post)
                .WithMany(p => p.PostTags)
                .HasForeignKey(pt => pt.PostId);

            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.PostTags)
                .HasForeignKey(pt => pt.TagId);

            modelBuilder.Entity<Category>().HasData(new Category
            {
                Id = 1,
                Name = "General"
            });

            modelBuilder.Entity<Tag>().HasData(new Tag
            {
                Id = 1,
                Name = "misc."
            });

            modelBuilder.Entity<Post>().HasData(new Post
            {
                Id = 1,
                Title = "The blogs first post!",
                Content = "This is the first blog post!",
                Date = DateTime.Now,
                EditDate = null,
                CategoryId = 1
            });
        }
    }
}

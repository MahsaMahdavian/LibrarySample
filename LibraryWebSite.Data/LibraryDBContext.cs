using System;
using System.Collections.Generic;
using System.Text;
using LibraryWebSite.Data.Mapping;
using LibraryWebSite.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebSite.Data
{
    public class LibraryDBContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(local);Database=LibraryDB;Trusted_Connection=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.AddCustomLibraryWebsiteMappings();
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Author_Book> Author_Books { get; set; } 
        public DbSet<Language> Languages { get; set; }
        public DbSet<Translator> Translator { get; set; }
        public DbSet<Book_Category> Book_Categories { get; set; }
        public DbSet<Book_Translator> Book_Translators { get; set; }
    }
}

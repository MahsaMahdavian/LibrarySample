using System;
using System.Collections.Generic;
using System.Text;
using LibraryWebSite.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryWebSite.Data.Mapping
{
    class Book_CategoryMap : IEntityTypeConfiguration<Book_Category>
    {
        public void Configure(EntityTypeBuilder<Book_Category> builder)
        {
            builder.HasKey(t => new { t.BookID, t.CategoryID });
            builder
                .HasOne(p => p.Book)
                .WithMany(c => c.book_Categories)
                .HasForeignKey(f => f.BookID);

            builder
               .HasOne(p => p.Category)
               .WithMany(c => c.book_Categories)
               .HasForeignKey(f => f.CategoryID);
        }
    }

}

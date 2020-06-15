using System;
using System.Collections.Generic;
using System.Text;
using LibraryWebSite.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryWebSite.Data.Mapping
{
    public class Author_BookMap: IEntityTypeConfiguration<Author_Book>
    {
        public void Configure(EntityTypeBuilder<Author_Book> builder)
        {
            builder.HasKey(t => new { t.BookID, t.AuthorID });
            builder
              .HasOne(p => p.Book)
              .WithMany(t => t.author_Books)
              .HasForeignKey(f => f.BookID);

            builder
               .HasOne(p => p.Author)
               .WithMany(t => t.author_Books)
               .HasForeignKey(f => f.AuthorID);
        }
    }
}

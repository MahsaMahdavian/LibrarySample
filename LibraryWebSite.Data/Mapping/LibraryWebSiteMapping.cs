using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebSite.Data.Mapping
{
    public static class LibraryWebSiteMapping
    {
        public static void AddCustomLibraryWebsiteMappings(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Book_CategoryMap());
            modelBuilder.ApplyConfiguration(new Book_TranslatorMap());
            modelBuilder.ApplyConfiguration(new Author_BookMap());
           
        }
    }
}

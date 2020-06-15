using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryWebSite.Entities
{
    public class Book_Category
    {
        public int BookID { get; set; }
        public int CategoryID { get; set; }

        public Book Book { get; set; }
        public Category Category { get; set; }
    }
}

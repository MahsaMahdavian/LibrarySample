using System;
using System.Collections.Generic;
using System.Text;
using LibraryWebSite.Entities;

namespace LibraryWebSite.ViewModel
{
    public class HomeViewModel
    {
        public IEnumerable<Book> BookList { get; set; }
        public IEnumerable<Category> CategoryList { get; set; }
    }
}

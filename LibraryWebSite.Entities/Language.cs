using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryWebSite.Entities
{
    public class Language
    {
        public int LanguageID { get; set; }
        public string LanguageName { get; set; }

        public List<Book> Books { get; set; }
    }
}

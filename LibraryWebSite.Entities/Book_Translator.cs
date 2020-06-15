using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryWebSite.Entities
{
    public class Book_Translator
    {
        public int TranslatorID { get; set; }
        public int BookID { get; set; }

        public Book Book { get; set; }
        public Translator Translator { get; set; }
    }
}

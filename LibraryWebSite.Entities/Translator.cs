using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibraryWebSite.Entities
{
    public class Translator
    {
        [Key]
        public int TranslatorID { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }

        public List<Book_Translator> book_Tranlators { get; set; }
    }
}

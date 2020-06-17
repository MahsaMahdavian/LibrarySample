using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LibraryWebSite.Entities
{
    public class Book
    {
        [Key]
        public int BookID { get; set; }

        [Required]
        public string Title { get; set; }
        public string Summary { get; set; }
        public int Price { get; set; }

        public string File { get; set; }
        public int NumOfPages { get; set; }
        public string ISBN { get; set; }
        public bool? IsLoan { get; set; }
        public DateTime? InsertDate { get; set; }
        public bool? Delete { get; set; }


        [Column(TypeName = "image")]
        public byte[] Image { get; set; }
        public int LanguageID { get; set; }
        public Language Language { get; set; }
        public List<Author_Book> author_Books { get; set; }       
        public List<Book_Translator> book_Tranlators { get; set; }
        public List<Book_Category> book_Categories { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace LibraryWebSite.ViewModel
{
    public class BooksCreatViewModel
    {
        public int BookID { get; set; }

        public DateTime? InsertDate { get; set; }


        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        [Display(Name = "عنوان ")]
        public string Title { get; set; }


        [Display(Name = "خلاصه")]
        public string Summary { get; set; }
      

        [Display(Name = "تعداد صفحات")]
        public int NumOfPages { get; set; }


        [Display(Name = "شابک")]
        public string ISBN { get; set; }

        [Display(Name = " این کتاب موجود نیست.")]
        public bool IsLoan { get; set; }

        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        [Display(Name = "زبان")]
        public int LanguageID { get; set; }
 
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        [Display(Name = "نویسندگان")]
        public int[] AuthorID { get; set; }

        public IFormFile Image { get; set; }

        [Display(Name = "مترجمان")]
        public int[] TranslatorID { get; set; }
  
    }
      public class AuthorList
    {
        public int AuthorID { get; set; }
        public string NameFamily { get; set; }
    }

    public class TranslatorList
    {
        public int TranslatorID { get; set; }
        public string NameFamily { get; set; }
    }

    public class BooksAdvancedSearch
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string NumOfPages { get; set; }
        public string ISBN { get; set; }
        public string InsertDate { get; set; }            
        public string Language { get; set; }       
        public string Category { get; set; }
        public string Author { get; set; }
        public string Translator { get; set; }
    }


    public class ReadAllBook
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public int Price { get; set; }
        public int NumOfPages { get; set; }
        public short Weight { get; set; }
        public string ISBN { get; set; }
        public bool IsLoan { get; set; }
        public string LanguageName { get; set; }
        public string Authors { get; set; }
        public string Translators { get; set; }
        public string Categories { get; set; }
        public DateTime? InsertDate { get; set; }
        public byte[] Image { get; set; }



    }


}

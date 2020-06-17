using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryWebSite.Data.Contract;
using LibraryWebSite.Entities;
using LibraryWebSite.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LibraryWebSite.Controllers
{
    public class BookController : Controller
    {
        private readonly IUnitOfWork _UW;
        public BookController(IUnitOfWork UW)
        {
            _UW = UW;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Insert()
        {
            ViewBag.LanguageID = new SelectList(_UW.BaseRepository<Language>().FindAll(), "LanguageID", "LanguageName");
            ViewBag.AuthorID = new SelectList(_UW.BaseRepository<Author>().FindAll().Select(t => new AuthorList { AuthorID = t.AuthorID, NameFamily = t.FirstName + " " + t.LastName }), "AuthorID", "NameFamily");
            ViewBag.TranslatorID = new SelectList(_UW.BaseRepository<Translator>().FindAll().Select(t => new TranslatorList { TranslatorID = t.TranslatorID, NameFamily = t.Name + " " + t.Family }), "TranslatorID", "NameFamily");

            BooksCreatViewModel ViewModel = new BooksCreatViewModel();
            return View(ViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Insert(BooksCreatViewModel ViewModel)
        {
            if (ModelState.IsValid)
            {
                List<Book_Translator> translators = new List<Book_Translator>();
                List<Book_Category> categories = new List<Book_Category>();
                if (ViewModel.TranslatorID != null)
                    translators = ViewModel.TranslatorID.Select(a => new Book_Translator { TranslatorID = a }).ToList();
                if (ViewModel.CategoryID != null)
                    categories = ViewModel.CategoryID.Select(a => new Book_Category { CategoryID = a }).ToList();

                DateTime? InsertDate = null;


                InsertDate = DateTime.Now;

                Book book = new Book()
                {
                    Delete = false,
                    ISBN = ViewModel.ISBN,
                    IsLoan = ViewModel.IsLoan,
                    NumOfPages = ViewModel.NumOfPages,
                    LanguageID = ViewModel.LanguageID,
                    Summary = ViewModel.Summary,
                    Title = ViewModel.Title,
                    InsertDate = InsertDate,
                    author_Books = ViewModel.AuthorID.Select(a => new Author_Book { AuthorID = a }).ToList(),
                    book_Tranlators = translators,
                    book_Categories = categories,
                };

                await _UW.BaseRepository<Book>().CreateAsync(book);
                await _UW.Commit();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.LanguageID = new SelectList(_UW.BaseRepository<Language>().FindAll(), "LanguageID", "LanguageName");
                ViewBag.AuthorID = new SelectList(_UW.BaseRepository<Author>().FindAll().Select(t => new AuthorList { AuthorID = t.AuthorID, NameFamily = t.FirstName + " " + t.LastName }), "AuthorID", "NameFamily");
                ViewBag.TranslatorID = new SelectList(_UW.BaseRepository<Translator>().FindAll().Select(t => new TranslatorList { TranslatorID = t.TranslatorID, NameFamily = t.Name + " " + t.Family }), "TranslatorID", "NameFamily");
                return View(ViewModel);
            }
        }

        #region Api Calls

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            //return Json(new { data = _UW.BaseRepository<Book>().FindAll().Select(a=> new BooksAdvancedSearch 
            //{Title=a.Title,Summary=a.Summary,ISBN=a.ISBN,Language=a.Language.LanguageName })  });

            var aa = Json(new
            {
                data = _UW._Context.Books.Include(b => b.author_Books).ThenInclude(b => b.Author)
                                      .Include(b => b.book_Tranlators).ThenInclude(b => b.Translator)
                                      .Include(b => b.Language).ToList().Select(a => new BooksAdvancedSearch
                                      {
                                          Title = a.Title,
                                          Summary = a.Summary,
                                          NumOfPages = a.NumOfPages.ToString(),
                                          ISBN = a.ISBN,
                                          InsertDate=a.InsertDate.ToString(),
                                          Language = a.Language.LanguageName,
                                          Author = a.author_Books.Select(c => c.Author.FirstName + " " + c.Author.LastName).FirstOrDefault(),
                                          Translator=a.book_Tranlators.Select(t=>t.Translator.Name + " " + t.Translator.Family).FirstOrDefault() })
            }) ;
            
            return aa;
        }
        #endregion
    }
}


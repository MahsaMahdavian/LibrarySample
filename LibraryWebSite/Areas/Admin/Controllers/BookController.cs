using System;
using System.Collections.Generic;
using System.IO;
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
using Newtonsoft.Json.Linq;

namespace LibraryWebSite.Controllers
{
    [Area("Admin")]
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
                    
                };
                if (ViewModel.Image!=null)
                {
                    using(var memoryStream=new MemoryStream())
                    {
                        await ViewModel.Image.CopyToAsync(memoryStream);
                        book.Image = memoryStream.ToArray();
                    }
                }
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

        public IActionResult Details(int id)
        {
            if (id == 0)
                return NotFound();

            var BookInfo=_UW._Context.Books.Where(b=>b.BookID==id).Select(t => new ReadAllBook 
            { Title = t.Title,
                Summary = t.Summary ,
                NumOfPages=t.NumOfPages,
                InsertDate=t.InsertDate,
                IsLoan=(bool)t.IsLoan,
                ISBN=t.ISBN,
                Price=t.Price,
                BookID=t.BookID,
                Image=t.Image,
                LanguageName=t.Language.LanguageName,
                Authors=t.author_Books.Select(a=>a.Author.FirstName + " " + a.Author.LastName).FirstOrDefault(),
                Translators=t.book_Tranlators.Select(t=>t.Translator.Name+" "+t.Translator.Family).FirstOrDefault(),
                                                
            }).SingleOrDefault();                    
            return View(BookInfo);
        }

        public async Task<IActionResult> ViewImage(int id)
        {
            var Book =await _UW.BaseRepository<Book>().FindByIdAsync(id);
            if (Book == null)
                return NotFound();

            var memoryStream = new MemoryStream(Book.Image);
            return new FileStreamResult(memoryStream, "image/png");
        }

        #region Api Calls

        [HttpGet]
        public IActionResult GetAllBooks()
        {
          
            return Json(new
            {
                data = _UW._Context.Books.Include(b => b.author_Books).ThenInclude(b => b.Author)
                                      .Include(b => b.book_Tranlators).ThenInclude(b => b.Translator)
                                      .Include(b => b.Language).ToList().Select(a => new BooksAdvancedSearch
                                      {
                                          Id=a.BookID,
                                          Title = a.Title,
                                          Summary = a.Summary,
                                          NumOfPages = a.NumOfPages.ToString(),
                                          ISBN = a.ISBN,
                                          InsertDate=a.InsertDate.ToString(),
                                          Language = a.Language.LanguageName,
                                          Author = a.author_Books.Select(c => c.Author.FirstName + " " + c.Author.LastName).FirstOrDefault(),
                                          Translator=a.book_Tranlators.Select(t=>t.Translator.Name + " " + t.Translator.Family).FirstOrDefault() })
            }) ;
            
            
        }
        #endregion
    }
}


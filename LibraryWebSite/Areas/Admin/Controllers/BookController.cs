using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LibraryWebSite.Data.Contract;
using LibraryWebSite.Entities;
using LibraryWebSite.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


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
        [Authorize]
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
                    UserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier))
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
                UserID=t.UserID,
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


        public async Task<IActionResult> Delete(int id)
        {
            var Book = await _UW.BaseRepository<Book>().FindByIdAsync(id);
            if (Book != null)
            {
                Book.Delete = true;
                await _UW.Commit();
                return RedirectToAction("Index");
            }

            else
            {
                return NotFound();
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            else
            {
                var Book = await _UW.BaseRepository<Book>().FindByIdAsync(id);
                if (Book == null)
                {
                    return NotFound();
                }

                else
                {
                    var ViewModel = (from b in _UW._Context.Books.Include(l => l.Language)
                                     where (b.BookID == id)
                                     select new BooksCreatViewModel
                                     {
                                         BookID = b.BookID,
                                         Title = b.Title,
                                         ISBN = b.ISBN,
                                         NumOfPages = b.NumOfPages,                          
                                         LanguageID = b.LanguageID,                                    
                                         Summary = b.Summary,                                    
                                         IsLoan = (bool)b.IsLoan,
                                         InsertDate = b.InsertDate,
                                         

                                     }).FirstAsync();

                    int[] AuthorsArray = await (from a in _UW._Context.Author_Books
                                                where (a.BookID == id)
                                                select a.AuthorID).ToArrayAsync();

                    int[] TranslatorsArray = await (from t in _UW._Context.Book_Translators
                                                    where (t.BookID == id)
                                                    select t.TranslatorID).ToArrayAsync();                 

                    ViewModel.Result.AuthorID = AuthorsArray;
                    ViewModel.Result.TranslatorID = TranslatorsArray;

                    ViewBag.LanguageID = new SelectList(_UW.BaseRepository<Language>().FindAll(), "LanguageID", "LanguageName");
                    ViewBag.AuthorID = new SelectList(_UW.BaseRepository<Author>().FindAll().Select(t => new AuthorList { AuthorID = t.AuthorID, NameFamily = t.FirstName + " " + t.LastName }), "AuthorID", "NameFamily");
                    ViewBag.TranslatorID = new SelectList(_UW.BaseRepository<Translator>().FindAll().Select(t => new TranslatorList { TranslatorID = t.TranslatorID, NameFamily = t.Name + " " + t.Family }), "TranslatorID", "NameFamily");
                 
                    return View(await ViewModel);
                }
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BooksCreatViewModel ViewModel)
        {
            ViewBag.LanguageID = new SelectList(_UW.BaseRepository<Language>().FindAll(), "LanguageID", "LanguageName");
            ViewBag.AuthorID = new SelectList(_UW.BaseRepository<Author>().FindAll().Select(t => new AuthorList { AuthorID = t.AuthorID, NameFamily = t.FirstName + " " + t.LastName }), "AuthorID", "NameFamily");
            ViewBag.TranslatorID = new SelectList(_UW.BaseRepository<Translator>().FindAll().Select(t => new TranslatorList { TranslatorID = t.TranslatorID, NameFamily = t.Name + " " + t.Family }), "TranslatorID", "NameFamily");
           
            if (ModelState.IsValid)
            {
                try
                {
                    Book book = new Book()
                    {
                        BookID = ViewModel.BookID,
                        Title = ViewModel.Title,
                        ISBN = ViewModel.ISBN,
                        NumOfPages = ViewModel.NumOfPages,
                        IsLoan = ViewModel.IsLoan,
                        LanguageID = ViewModel.LanguageID,
                        Summary = ViewModel.Summary,
                        Delete = false,
                    };

                    _UW.BaseRepository<Book>().Update(book);

                    var RecentAuthors = (from a in _UW._Context.Author_Books
                                         where (a.BookID == ViewModel.BookID)
                                         select a.AuthorID).ToArray();

                    var RecentTranslators = (from a in _UW._Context.Book_Translators
                                             where (a.BookID == ViewModel.BookID)
                                             select a.TranslatorID).ToArray();

             
                    var DeletedAuthors = RecentAuthors.Except(ViewModel.AuthorID);
                    var DeletedTranslators = RecentTranslators.Except(ViewModel.TranslatorID);


                    var AddedAuthors = ViewModel.AuthorID.Except(RecentAuthors);
                    var AddedTranslators = ViewModel.TranslatorID.Except(RecentTranslators);
                   

                    if (DeletedAuthors.Count() != 0)
                        _UW.BaseRepository<Author_Book>().DeleteRange(DeletedAuthors.Select(a => new Author_Book { AuthorID = a, BookID = ViewModel.BookID }).ToList());

                    if (DeletedTranslators.Count() != 0)
                        _UW.BaseRepository<Book_Translator>().DeleteRange(DeletedTranslators.Select(a => new Book_Translator { TranslatorID = a, BookID = ViewModel.BookID }).ToList());

                   
                    if (AddedAuthors.Count() != 0)
                        await _UW.BaseRepository<Author_Book>().CreateRangeAsync(AddedAuthors.Select(a => new Author_Book { AuthorID = a, BookID = ViewModel.BookID }).ToList());

                    if (AddedTranslators.Count() != 0)
                        await _UW.BaseRepository<Book_Translator>().CreateRangeAsync(AddedTranslators.Select(a => new Book_Translator { TranslatorID = a, BookID = ViewModel.BookID }).ToList());

                    await _UW.Commit();

                    ViewBag.MsgSuccess = "ذخیره تغییرات با موفقیت انجام شد.";
                    return View(ViewModel);
                }

                catch
                {
                    ViewBag.MsgFailed = "در ذخیره تغییرات خطایی رخ داده است.";
                    return View(ViewModel);
                }
            }

            else
            {
                ViewBag.MsgFailed = "اطلاعات فرم نامعتبر است.";
                return View(ViewModel);
            }
        }



        #region Api Calls

        [HttpGet]
        public IActionResult GetAllBooks()
        {
          
            return Json(new
            {
                data = _UW._Context.Books.Include(b => b.author_Books).ThenInclude(b => b.Author)
                                      .Include(b => b.book_Tranlators).ThenInclude(b => b.Translator)
                                      .Include(b => b.Language).Where(b=>b.Delete==false).ToList().Select(a => new BooksAdvancedSearch
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


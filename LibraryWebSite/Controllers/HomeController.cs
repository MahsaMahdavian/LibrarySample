using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LibraryWebSite.Models;
using LibraryWebSite.Data.Contract;
using LibraryWebSite.ViewModel;
using LibraryWebSite.Entities;

namespace LibraryWebSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _UW;
        private HomeViewModel homeViewModel;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork UW)
        {
            _logger = logger;
            _UW = UW;
        }

        public IActionResult Index()
        {
            homeViewModel = new HomeViewModel()
            {
                CategoryList = _UW.BaseRepository<Category>().FindAll(),
                //  BookList = _UW.BaseRepository<Book>().FindByConditionAsync(null,null,a=>a.book_Tranlators)
                BookList = _UW.BaseRepository<Book>().FindAll()
            };
            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

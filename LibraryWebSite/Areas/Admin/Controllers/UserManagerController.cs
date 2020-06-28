using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryWebSite.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;

namespace LibraryWebSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UserManagerController : Controller
    {
        private readonly IApplicationUserManager _userManager;
        private readonly IApplicationRoleManager _roleManager;

        public UserManagerController(IApplicationUserManager userManager, IApplicationRoleManager roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult>  Index(string Msg, int page = 1, int row = 10)
        {
            if (Msg == "Success")
                ViewBag.Alert = "عضویت با موفقیت انجام شد.";

            

            var PagingModel = PagingList.Create(await _userManager.GetAllUsersWithRolesAsync(), row, page);
            return View(PagingModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
                return NotFound();
            else
            {
                var User = await _userManager.FindUserWithRolesByIdAsync(id);
                if (User == null)
                    return NotFound();
                else
                    return View(User);
            }
        }
    }
}
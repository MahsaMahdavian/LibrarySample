﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryWebSite.Common;
using LibraryWebSite.Entities.Identity;
using LibraryWebSite.Services.Contracts;
using LibraryWebSite.ViewModel.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

        public async Task<IActionResult> Index(string Msg, int page = 1, int row = 10)
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


        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();
            var User = await _userManager.FindByIdAsync(id);
            if (User == null)
                return NotFound();
            else
            {
                var Result = await _userManager.DeleteAsync(User);
                if (Result.Succeeded)
                    return RedirectToAction("Index");
                else
                    ViewBag.AlertError = "در حذف اطلاعات خطایی رخ داده است.";

                return View(User);
            }
        }


        
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
                return NotFound();
            var User = await _userManager.FindUserWithRolesByIdAsync(id);
            if (User == null)
                return NotFound();
            else
            {
                ViewBag.AllRoles = _roleManager.GetAllRoles();
                if (User.BirthDate != null)
                    User.PersianBirthDate = User.BirthDate.ConvertMiladiToShamsi("yyyy/MM/dd");

                return View(User);
            }
        }



    
    }
}
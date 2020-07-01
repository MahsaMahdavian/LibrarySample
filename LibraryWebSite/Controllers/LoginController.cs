using System.Threading.Tasks;
using LibraryWebSite.Entities.Identity;
using LibraryWebSite.Services.Contracts;
using LibraryWebSite.ViewModel;
using LibraryWebSite.ViewModel.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity.UI.Services;
using LibraryWebSite.Common;
using System;

namespace LibraryWebSite.Controllers
{
    
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly IApplicationUserManager _userManager;
        private readonly IApplicationRoleManager _roleManager;
        private readonly IHttpContextAccessor _accessor;
        private readonly IEmailSender _emailSender;
        public LoginController
            (
            ILogger<LoginController> logger,
            SignInManager<User> signInManager,
            IApplicationUserManager userManager,
            IApplicationRoleManager roleManager,
            IHttpContextAccessor accessor,        
            IEmailSender emailSender

            )
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _accessor = accessor;
            _emailSender = emailSender;

        }
       
      
        public IActionResult Index(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel viewModel, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByNameAsync(viewModel.UserName);
                if (User != null)
                {
                    if (User.IsActive)
                    {
                        var result = await _signInManager.PasswordSignInAsync(viewModel.UserName, viewModel.Password, viewModel.RememberMe, true);
                        if (result.Succeeded)
                        {
                            _logger.LogInformation(1, $"{viewModel.UserName} logged in.");
                            if (Url.IsLocalUrl(returnUrl))
                            {
                                return Redirect(returnUrl);
                            }
                            return RedirectToAction("Index","Book", new { area = "Admin" });
                        }

                        else if (result.IsLockedOut)
                            ModelState.AddModelError(string.Empty, "حساب کاربری شما به مدت 20 دقیقه به دلیل تلاش های ناموفق قفل شد.");

                        else if (result.RequiresTwoFactor)
                            return View(viewModel);

                        else
                        {
                            _logger.LogWarning($"The user attempts to login with the IP address({_accessor.HttpContext?.Connection?.RemoteIpAddress.ToString()}) and username ({viewModel.UserName}) and password ({viewModel.Password}).");
                            ModelState.AddModelError(string.Empty, "نام کاربری یا کلمه عبور شما صحیح نمی باشد.");
                        }
                    }
                    else
                        ModelState.AddModelError(string.Empty, "حساب کاربری شما غیرفعال است.");
                }

                else
                {
                    _logger.LogWarning($"The user attempts to login with the IP address({_accessor.HttpContext?.Connection?.RemoteIpAddress.ToString()}) and username ({viewModel.UserName}) and password ({viewModel.Password}).");
                    ModelState.AddModelError(string.Empty, "نام کاربری یا کلمه عبور شما صحیح نمی باشد.");
                }
            }

            return View(viewModel); 
        }


        public async Task<IActionResult> SignOut()
        {
            var user = User.Identity.IsAuthenticated ? await _userManager.FindByNameAsync(User.Identity.Name) : null;
            await _signInManager.SignOutAsync();
            if (user != null)
            {
                await _userManager.UpdateAsync(user);
                _logger.LogInformation(4, $"{user.UserName} logged out.");
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel ViewModel)
        {
            if (ModelState.IsValid)
            {
                DateTime BirthDateMiladi = ViewModel.BirthDate.ConvertShamsiToMiladi();

                var user = new User 
                {
                    UserName = ViewModel.UserName,
                    Email = ViewModel.Email, 
                    PhoneNumber = ViewModel.PhoneNumber,
                    CreatedDateTime = DateTime.Now, 
                    IsActive = true,
                    BirthDate = BirthDateMiladi 
                };
                IdentityResult result = await _userManager.CreateAsync(user, ViewModel.Password);

                if (result.Succeeded)
                {
                    var role =await _roleManager.FindByNameAsync("User");
                    if (role == null)
                        await _roleManager.CreateAsync(new Role("User"));

                    result = await _userManager.AddToRoleAsync(user, "User");
                    await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.DateOfBirth, BirthDateMiladi.ToString("MM/dd")));

                    if (result.Succeeded)
                    {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var result1 = await _userManager.ConfirmEmailAsync(user, code);
                        var callbackUrl = Url.Action("ConfirmEmail", "Login", values: new { userId = user.Id, code = code }, protocol: Request.Scheme);

                       //await _emailSender.SendEmailAsync(ViewModel.Email, "تایید ایمیل حساب کاربری - ", $"<div dir='rtl' style='font-family:tahoma;font-size:14px'>لطفا با کلیک روی لینک رویه رو ایمیل خود را تایید کنید.  <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>کلیک کنید</a></div>");

                        return RedirectToAction("Index", "Home", new { id = "ConfirmEmail" });
                    }
                }

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
            }

            return View();
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
                return RedirectToAction("Index", "Home");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound($"Unable to load user with ID '{userId}'");

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
                throw new InvalidOperationException($"Error Confirming email for user with ID '{userId}'");

            return View();
        }

        [Authorize(Policy = "HappyBirthDay")]
        public IActionResult HappyBirthDay()
        {
            return View();
        }
    }
}
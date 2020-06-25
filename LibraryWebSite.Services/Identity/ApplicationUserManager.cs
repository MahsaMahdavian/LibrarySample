﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LibraryWebSite.Entities.Identity;
using LibraryWebSite.ViewModel.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LibraryWebSite.Services.Contracts
{
    public class ApplicationUserManager : UserManager<User>, IApplicationUserManager
    {
        private readonly CustomIdentityErrorDescriber _errors;
        private readonly ILookupNormalizer _keyNormalizer;
        private readonly ILogger<ApplicationUserManager> _logger;
        private readonly IOptions<IdentityOptions> _options;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IEnumerable<IPasswordValidator<User>> _passwordValidators;
        private readonly IServiceProvider _services;
        private readonly IUserStore<User> _userStore;
        private readonly IEnumerable<IUserValidator<User>> _userValidators;
       

        public ApplicationUserManager(
            CustomIdentityErrorDescriber errors,
            ILookupNormalizer keyNormalizer,
            ILogger<ApplicationUserManager> logger,
            IOptions<IdentityOptions> options,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            IServiceProvider services,
            IUserStore<User> userStore,
            IEnumerable<IUserValidator<User>> userValidators
            )
            : base(userStore, options, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _userStore = userStore;
            _errors = errors;
            _logger = logger;
            _services = services;
            _passwordHasher = passwordHasher;
            _userValidators = userValidators;
            _options = options;
            _keyNormalizer = keyNormalizer;
            _passwordValidators = passwordValidators;
           
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await Users.ToListAsync();
        }

        public async Task<List<UsersViewModel>> GetAllUsersWithRolesAsync()
        {
            return await Users.Select(user => new UsersViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                IsActive = user.IsActive,
                Image = user.PhotoFileName,
                CreateDateTime = user.CreatedDateTime,
                Roles = user.Roles,

            }).ToListAsync();
        }

        public async Task<UsersViewModel> FindUserWithRolesByIdAsync(int UserId)
        {
            return await Users.Where(u => u.Id == UserId).Select(user => new UsersViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                IsActive = user.IsActive,
                Image = user.PhotoFileName,
                CreateDateTime = user.CreatedDateTime,
                RoleName = user.Roles.First().Role.Name,
                AccessFailedCount = user.AccessFailedCount,
                EmailConfirmed = user.EmailConfirmed,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEnd = user.LockoutEnd,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled,
                Gender = user.Gender,
            }).FirstOrDefaultAsync();
        }

        public async Task<string> GetFullName(ClaimsPrincipal User)
        {
            var UserInfo = await GetUserAsync(User);
            return UserInfo.FirstName + " " + UserInfo.LastName;
        }


        //public List<UsersViewModel> GetPaginateUsers(int offset, int limit, Func<UsersViewModel, Object> orderByAscFunc, Func<UsersViewModel, Object> orderByDescFunc, string searchText)
        //{
        //    var users = Users.Include(u => u.Roles).Where(t => t.FirstName.Contains(searchText) || t.LastName.Contains(searchText) || t.Email.Contains(searchText) || t.UserName.Contains(searchText) || t.CreatedDateTime.ConvertMiladiToShamsi("yyyy/MM/dd ساعت HH:mm:ss").Contains(searchText))
        //          .Select(user => new UsersViewModel
        //          {
        //              Id = user.Id,
        //              Email = user.Email,
        //              UserName = user.UserName,
        //              PhoneNumber = user.PhoneNumber,
        //              FirstName = user.FirstName,
        //              LastName = user.LastName,
        //              IsActive = user.IsActive,
        //              Image = user.Image,
        //              PersianBirthDate = user.BirthDate.ConvertMiladiToShamsi("yyyy/MM/dd"),
        //              PersianRegisterDateTime = user.RegisterDateTime.ConvertMiladiToShamsi("yyyy/MM/dd ساعت HH:mm:ss"),
        //              GenderName = user.Gender == GenderType.Male ? "مرد" : "زن",
        //              RoleId = user.Roles.Select(r => r.Role.Id).FirstOrDefault(),
        //              RoleName = user.Roles.Select(r => r.Role.Name).FirstOrDefault()
        //          }).OrderBy(orderByAscFunc).OrderByDescending(orderByDescFunc).Skip(offset).Take(limit).ToList();

        //    foreach (var item in users)
        //        item.Row = ++offset;

        //    return users;
        //}


        public string CheckAvatarFileName(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName);
            int fileNameCount = Users.Where(f => f.PhotoFileName == fileName).Count();
            int j = 1;
            while (fileNameCount != 0)
            {
                fileName = fileName.Replace(fileExtension, "") + j + fileExtension;
                fileNameCount = Users.Where(f => f.PhotoFileName == fileName).Count();
                j++;
            }

            return fileName;
        }

        public string NormalizeKey(string key)
        {
            throw new NotImplementedException();
        }
    }
}

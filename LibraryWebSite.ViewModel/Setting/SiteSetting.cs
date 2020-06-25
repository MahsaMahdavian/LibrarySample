using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryWebSite.ViewModel.Setting
{
    public class SiteSetting
    {
        public AdminUserSeed AdminUserSeed { get; set; }
    }
        public class AdminUserSeed
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public string RoleName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }


using System;
using System.Collections.Generic;
using System.Text;
using LibraryWebSite.Entities.AuditableEntity;
using Microsoft.AspNetCore.Identity;

namespace LibraryWebSite.Entities.Identity
{
    public class UserClaim : IdentityUserClaim<int>, IAuditableEntity
    {
        public virtual User User { get; set; }
    }
}

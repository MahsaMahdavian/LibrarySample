using System;
using System.Collections.Generic;
using System.Text;
using LibraryWebSite.Entities.AuditableEntity;
using Microsoft.AspNetCore.Identity;

namespace LibraryWebSite.Entities.Identity
{
    public class RoleClaim : IdentityRoleClaim<int>, IAuditableEntity
    {
        public virtual Role Role { get; set; }
    }
}

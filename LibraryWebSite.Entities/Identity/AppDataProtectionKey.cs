using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryWebSite.Entities.Identity
{
    public class AppDataProtectionKey
    {
        public int Id { get; set; }
        public string FriendlyName { get; set; }
        public string XmlData { get; set; }
    }
}

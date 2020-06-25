using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LibraryWebSite.Services.Identity
{
    public interface IIdentityDbInitializer
    {
        void Initialize();
        void SeedData();
        Task<IdentityResult> SeedDatabaseWithAdminUserAsync();
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryWebSite.Entities.Identity;
using LibraryWebSite.ViewModel.Identity;
using Microsoft.AspNetCore.Identity;

namespace LibraryWebSite.Services.Contracts
{
    public interface IApplicationRoleManager
    {
        #region BaseClass
        IQueryable<Role> Roles { get; }
        ILookupNormalizer KeyNormalizer { get; set; }
        IdentityErrorDescriber ErrorDescriber { get; set; }
        IList<IRoleValidator<Role>> RoleValidators { get; }
        bool SupportsQueryableRoles { get; }
        bool SupportsRoleClaims { get; }
        Task<IdentityResult> CreateAsync(Role role);
        Task<IdentityResult> DeleteAsync(Role role);
        Task<Role> FindByIdAsync(string roleId);
        Task<Role> FindByNameAsync(string roleName);
        string NormalizeKey(string key);
        Task<bool> RoleExistsAsync(string roleName);
        Task<IdentityResult> UpdateAsync(Role role);
        Task UpdateNormalizedRoleNameAsync(Role role);
        Task<string> GetRoleNameAsync(Role role);
        Task<IdentityResult> SetRoleNameAsync(Role role, string name);
        #endregion

        #region CustomMethod
        Task<IdentityResult> AddOrUpdateClaimsAsync(int roleId, string roleClaimType, IList<string> SelectedRoleClaimValues);
        Task<Role> FindClaimsInRole(int roleId);
        List<Role> GetAllRoles();
        List<RolesViewModel> GetAllRolesAndUsersCount();
        Task<List<RolesViewModel>> GetPaginateRolesAsync(int offset, int limit, bool? roleNameSortAsc, string searchText);
        Task<List<UsersViewModel>> GetUsersInRoleAsync(int RoleId);
        #endregion

    }
}
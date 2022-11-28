using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindUsersByClaimsPrincipalWithAddressAsync(this UserManager<AppUser> input,
        ClaimsPrincipal claimUser)
        {
            var email = claimUser.FindFirstValue(ClaimTypes.Email);
            return await input.Users.Include(u => u.Address).SingleOrDefaultAsync(x => x.Email == email);
        }

        public static async Task<AppUser> FindEmailFromClaimsPrincipal(this UserManager<AppUser> input, ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);
            return await input.Users.SingleOrDefaultAsync(x => x.Email == email);
        }
    }
}
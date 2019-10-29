using FrontEnd.Data;
using FrontEnd.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FrontEnd.Helpers;

namespace FrontEnd.Areas.Identity
{
    public class ClaimsPrincipalFactory : UserClaimsPrincipalFactory<User>
    {
        private readonly ApiClient apiClient;

        public ClaimsPrincipalFactory(ApiClient apiClient, UserManager<User> userManager, IOptions<IdentityOptions> options) : base(userManager, options)
        {
            this.apiClient = apiClient;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            
            var identity = await base.GenerateClaimsAsync(user);
            if (user.IsAdmin) identity.MakeAdmin();
            return identity;
        }
    }
}

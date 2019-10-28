using FrontEnd.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.Services
{
    public class AdminService
    {
        private readonly IdentityContext context;
        public AdminService(IdentityContext context)
        {
            this.context = context;
        }

        public async Task<bool> AllowAdminUserCreationAsync()
        {
            return await context.Users.AnyAsync(user => user.IsAdmin);
        }
    }
}

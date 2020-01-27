using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityTest.Models
{
    public class UserDbContext :IdentityDbContext<AppUser,IdentityRole<int>,int>
    {
        public UserDbContext(DbContextOptions options):base(options)
        {

        }
    }
}

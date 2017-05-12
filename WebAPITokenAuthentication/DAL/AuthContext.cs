using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebAPITokenAuthentication.DAL
{
    public class AuthContext : IdentityDbContext<IdentityUser>
    {
//             IdentityDbContext class, you can think about this class as special version of the traditional “DbContext” Class, 
//             it willprovide all of the Entity Framework code-first mapping and DbSet properties needed to manage the identity tables
//             in SQL Server

        public AuthContext() : base("AuthContext")
        {
        }
    }
}
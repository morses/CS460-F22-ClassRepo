using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleApp.Areas.Identity.Data;

namespace SimpleApp.Data;

public class ApplicationDbContext : IdentityDbContext<SimpleUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}

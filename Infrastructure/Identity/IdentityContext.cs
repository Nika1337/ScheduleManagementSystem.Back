using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity;

internal class IdentityContext(DbContextOptions<IdentityContext> options) : IdentityDbContext<IdentityEmployee, IdentityRole<Guid>, Guid>(options)
{

}

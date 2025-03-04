using Infrastructure.Data.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Identity;

internal class IdentityContext(DbContextOptions<IdentityContext> options) : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{

}

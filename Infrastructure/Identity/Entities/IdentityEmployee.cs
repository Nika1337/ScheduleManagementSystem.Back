
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Entities;

public class IdentityEmployee : IdentityUser<Guid>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }

}

using Microsoft.AspNetCore.Identity;

namespace Logimat.API.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public string email { get; set; }
    public string password { get; set; }

}

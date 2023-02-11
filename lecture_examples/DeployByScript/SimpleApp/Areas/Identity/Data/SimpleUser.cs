using Microsoft.AspNetCore.Identity;

namespace SimpleApp.Areas.Identity.Data;

public class SimpleUser : IdentityUser
{
    [PersonalData]
    public string FirstName { get; set; }
    [PersonalData]
    public string LastName { get; set; }
    [PersonalData]
    public DateTime DOB { get; set; }
}

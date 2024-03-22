using Microsoft.AspNetCore.Identity;

namespace COMP2139_Labs.Areas.ProjectManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }


        // sometimes people use in order to block the person who's trying to get in. i.,e when someone tries logging in 10 times
        public int UsernameChangeLimit { get; set; } = 10;
        public byte[]? ProfilePicture { get; set; }
    }
}

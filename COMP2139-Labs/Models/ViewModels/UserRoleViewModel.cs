namespace COMP2139_Labs.Models.ViewModels
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}

using CoEco.Front.Auth.Domain;

namespace CoEco.Front.Models.Account
{
    public class UserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TZ { get; set; }
        public string RequestToken { get; set; }
        public int UnitId { get; set; }

        public UserPermission Permissions { get; set; }
    }
}
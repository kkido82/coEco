namespace CoEco.Front.Auth.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public int UnitId { get; set; }
        public UserPermission Permissions { get; set; }

    }

    public class UserPermission
    {
        public bool CanOpenOrder { get; set; }
        public bool CanUpdateInventory { get; set; }
        public bool CanConfirmOrder { get; set; }
    }

}

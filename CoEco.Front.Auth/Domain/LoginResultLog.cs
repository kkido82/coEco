namespace CoEco.Front.Auth.Domain
{
    public class LoginResultLog : BaseEntity
    {
        public string Username { get; set; }
        public bool Succes { get; set; }
    }

}

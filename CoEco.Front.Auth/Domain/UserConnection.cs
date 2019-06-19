namespace CoEco.Front.Auth.Domain
{
    public class UserConnection : BaseEntity
    {
        public string Username { get; set; }
        public int NumActiveConnections { get; set; }
    }
}

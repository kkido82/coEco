using System;

namespace CoEco.Front.Auth.Domain
{
    public class Connection : BaseEntity
    {
        public string Username { get; set; }
        public DateTime ExpireDate { get; set; }
        public string ConnectionId { get; set; }
    }
}

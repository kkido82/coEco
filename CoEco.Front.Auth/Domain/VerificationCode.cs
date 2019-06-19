using System;

namespace CoEco.Front.Auth.Domain
{
    public class VerificationCode : BaseEntity
    {
        public string Username { get; set; }
        public string Code { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime? VerifiedOn { get; set; }
    }
}

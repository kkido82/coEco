using CoEco.Core.Context;
using System.Web;

namespace CoEco.Front.Services
{
    public class AppUserContext : IUserContext
    {
        private readonly HttpContext _context;

        public AppUserContext(HttpContext context)
        {
            _context = context;
        }

        public string UserName
        {
            get
            {
                var identity = _context.User.Identity;
                return "Anonymous";///TODO: Connect username from auth
                // identity.IsAuthenticated ? identity.GetUserName() : "Anonymous";
            }
        }
    }
}
using CoEco.Core.Context;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoEco.BO.Services
{
    public class BoUserContext : IUserContext
    {
        private readonly HttpContext _context;

        public BoUserContext(HttpContext context)
        {
            _context = context;
        }

        public string UserName
        {
            get
            {
                var identity = _context.User.Identity;
                return identity.IsAuthenticated ? identity.GetUserName() : "Anonymous";
            }
        }
    }
}
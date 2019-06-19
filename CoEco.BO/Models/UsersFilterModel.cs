using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoEco.BO.Models
{
    public class UsersFilterModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }

    }
}
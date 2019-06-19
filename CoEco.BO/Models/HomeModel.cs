using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoEco.BO.Models
{
    public class HomeModel
    {
        public string Title { get; set; }
        public UserModel User { get; set; }

        public class UserModel
        {
            public string UserName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string EmailAddress { get; set; }
        };

    }
}
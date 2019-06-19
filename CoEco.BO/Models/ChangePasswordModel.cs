using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoEco.BO.Models
{
    public class ChangePasswordModel
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }
}
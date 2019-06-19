using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoEco.BO.Models
{
    public class UsersListModel
    {
        public IList<UpdateUserModel> Users { get; set; }
        public int NumResults { get; set; }

        public UsersListModel()
        {
            Users = new List<UpdateUserModel>();
        }
    }
}
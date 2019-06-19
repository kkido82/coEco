using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace CoEco.BO.Models
{
    public class UpdateUserModel
    {
        [DisplayName(@"מזהה")]
        public string Id { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }


        //[EmailAddress(ErrorMessageResourceType = typeof(Resources.Validation), ErrorMessageResourceName = "InvalidEmail")]
        //[Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public bool Disabled { get; set; }
        public string UserName { get; set; }
        [RegularExpression(pattern: @"(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_]).+")]
        public string Password { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.Validation), ErrorMessageResourceName = "InvalidAccountExpiresAt")]
        public DateTime? AccountExpiresAt { get; set; }

        public Dictionary<string, AccessRight> Roles { get; set; }

        public UpdateUserModel()
        {
            Roles = new Dictionary<string, AccessRight>();
        }


    }

    public class UpdateUserResult
    {
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; }



        public UpdateUserResult()
        {
            Errors = new List<string>();
        }
    }
}
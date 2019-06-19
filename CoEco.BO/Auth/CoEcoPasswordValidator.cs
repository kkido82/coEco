using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Resources;
using System.Web.Helpers;

namespace CoEco.BO.Auth
{
    public class CoEcoPasswordValidator : PasswordValidator
    {
        public override Task<IdentityResult> ValidateAsync(string item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            var list = new List<string>();
            if (string.IsNullOrWhiteSpace(item) || item.Length < this.RequiredLength)
                list.Add(string.Format(CultureInfo.CurrentCulture, Resources.Validation.PasswordTooShort, new object[1]
                {
                  RequiredLength
                }));
            if (RequireNonLetterOrDigit && item.All(IsLetterOrDigit))
                list.Add(Resources.Validation.PasswordRequireNonLetterOrDigit);
            if (RequireDigit && item.All(c => !IsDigit(c)))
                list.Add(Resources.Validation.PasswordRequireDigit);
            if (RequireLowercase && item.All(c => !IsLower(c)))
                list.Add(Resources.Validation.PasswordRequireLower);
            if (RequireUppercase && item.All(c => !IsUpper(c)))
                list.Add(Resources.Validation.PasswordRequireUpper);
            if (list.Count == 0)
                return Task.FromResult(IdentityResult.Success);
            return Task.FromResult(IdentityResult.Failed(new string[1]
              {
                string.Join(" ", list)
              }));
        }
    }
    public class CoEcoUserValidator : UserValidator<ApplicationUser, string>
    {
        public CoEcoUserValidator(UserManager<ApplicationUser, string> manager) : base(manager)
        {
            this.Manager = manager;
        }

        private UserManager<ApplicationUser, string> Manager { get; set; }

        public override async Task<IdentityResult> ValidateAsync(ApplicationUser item)
        {
            if ((object)item == null)
                throw new ArgumentNullException("item");
            List<string> errors = new List<string>();
            await this.ValidateUserName(item, errors);
            if (this.RequireUniqueEmail && !string.IsNullOrWhiteSpace(item.Email))
                await this.ValidateEmailAsync(item, errors);
            return errors.Count <= 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
        }

        private async Task ValidateUserName(ApplicationUser user, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(user.UserName))
                errors.Add(string.Format((IFormatProvider)CultureInfo.CurrentCulture, UserValidation.PropertyTooShort, new object[1]
                {
          (object) "Name"
                }));
            else if (this.AllowOnlyAlphanumericUserNames && !Regex.IsMatch(user.UserName, "^[A-Za-z0-9@_\\.]+$"))
            {
                errors.Add(string.Format((IFormatProvider)CultureInfo.CurrentCulture, UserValidation.InvalidUserName, new object[1]
                {
          (object) user.UserName
                }));
            }
            else
            {
                ApplicationUser owner = await this.Manager.FindByNameAsync(user.UserName);
                if ((object)owner == null || object.Equals(owner.Id, user.Id))
                    return;
                errors.Add(string.Format((IFormatProvider)CultureInfo.CurrentCulture, UserValidation.DuplicateName, new object[1]
                {
          (object) user.UserName
                }));
            }
        }

        private async Task ValidateEmailAsync(ApplicationUser user, List<string> errors)
        {
            string email = user.Email;
            if (string.IsNullOrWhiteSpace(email))
            {
                errors.Add(string.Format((IFormatProvider)CultureInfo.CurrentCulture, UserValidation.PropertyTooShort, new object[1]
                {
          (object) "Email"
                }));
            }
            else
            {
                try
                {
                    MailAddress mailAddress = new MailAddress(email);
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (FormatException ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    errors.Add(string.Format((IFormatProvider)CultureInfo.CurrentCulture, UserValidation.InvalidEmail, new object[1]
                    {
            (object) email
                    }));
                    return;
                }
                ApplicationUser owner = await this.Manager.FindByEmailAsync(email);
                if ((object)owner == null || owner.Id == user.Id)
                    return;
                errors.Add(string.Format((IFormatProvider)CultureInfo.CurrentCulture, UserValidation.DuplicateEmail, new object[1]
                {
          (object) email
                }));
            }
        }
    }
}

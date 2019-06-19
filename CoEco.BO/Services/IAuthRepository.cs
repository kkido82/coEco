using CoEco.BO.Auth;
using CoEco.BO.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CoEco.BO.Services
{
    public interface IAuthRepository
    {
        Task<IdentityResult> RegisterUser(Models.UserModel userModel);
        Task<BO.Auth.ApplicationUser> FindUser(string userName, string password);
        Task<IdentityResult> Login(string userName, string password, bool isPersistent, bool rememberMe);
        Task<IdentityResult> ChangePassword(string userId, string password);
        Task<IdentityResult> UpdateUser(UpdateUserModel item);
        Task<IdentityResult> SSOCRMLogIn(string userName);
        Task<UsersListModel> List(string firstName, string lastName, string email, string userName,
            int skip, int take);

        Task<UpdateUserModel> GetById(string id);
        Task<IdentityResult> CreateUser(UpdateUserModel model);
        Task<IdentityResult> SaveUserPermissions(string id, Dictionary<string, AccessRight> roles);

        Task<IdentityResult> ForceUserToReload(string id, bool shouldsignout = true);
        Task<IdentityResult> SignOut();
    }
}
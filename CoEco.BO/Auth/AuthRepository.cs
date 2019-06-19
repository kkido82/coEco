using CoEco.BO.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoEco.BO.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using static CoEco.BO.Models.HomeModel;
using AutoMapper;
using System.Data.Entity;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;
using CoEco.BO.Config;
using System.Security.Claims;

namespace CoEco.BO.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationSignInManager _signInManager;
        private readonly ApplicationDbContext _db;
        private readonly IAuthenticationManager _authenticationManager;



        public AuthRepository(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationDbContext db, IAuthenticationManager authenticationManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
            _authenticationManager = authenticationManager;
        }

        public async Task<IdentityResult> RegisterUser(Models.UserModel userModel)
        {
            var user = new ApplicationUser
            {
                UserName = userModel.UserName,
                Email = userModel.Email
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            return result;
        }

        public async Task<ApplicationUser> FindUser(string userName, string password)
        {
            var user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public async Task<IdentityResult> SSOCRMLogIn(string userName)
        {

            try
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user == null)
                {
                    user = await _userManager.FindByNameAsync(userName);
                    if (user != null && await _userManager.GetLockoutEnabledAsync(user.Id)) //wrong credentials
                    {
                        await _userManager.AccessFailedAsync(user.Id);
                    }
                    throw new Exception("חשבון לא קיים או סיסמה שגויה");
                }

                if (user.LockoutEnabled && await _userManager.IsLockedOutAsync(user.Id))
                {
                    throw new Exception("חשבונך נעול");
                }

                await _signInManager.SignInAsync(user, true, false);
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                return IdentityResult.Failed(e.Message);
            }

        }

        public async Task<IdentityResult> Login(string userName, string password, bool isPersistent, bool rememberMe)
        {
            try
            {
                var user = await FindUser(userName, password);
                if (user == null)
                {
                    user = await _userManager.FindByNameAsync(userName);
                    if (user != null && await _userManager.GetLockoutEnabledAsync(user.Id)) //wrong credentials
                    {
                        await _userManager.AccessFailedAsync(user.Id);
                    }
                    throw new Exception("חשבון לא קיים או סיסמה שגויה");
                }

                if (user.LockoutEnabled && await _userManager.IsLockedOutAsync(user.Id))
                {
                    throw new Exception("חשבונך נעול");
                }

                await _signInManager.SignInAsync(user, isPersistent, rememberMe);

                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                return IdentityResult.Failed(e.Message);
            }
        }

        public async Task<IdentityResult> ChangePassword(string userId, string password)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(userId);
            var result = await _userManager.ResetPasswordAsync(userId, token, password);
            return result;

        }

        public async Task<IdentityResult> UpdateUser(UpdateUserModel item)
        {

            var user = await _userManager.FindByIdAsync(item.Id);
            if (user != null)
            {
                Mapper.Map(item, user);
                return await _userManager.UpdateAsync(user);
            }

            return IdentityResult.Failed("User Not Found");


        }

        public async Task<UsersListModel> List(string firstName, string lastName, string email, string userName,
            int skip, int take)
        {

            IQueryable<ApplicationUser> query = _db.Users.OrderByDescending(x => x.LastName);

            if (!string.IsNullOrWhiteSpace(firstName))
                query = query.Where(x => x.FirstName.Contains(firstName));

            if (!string.IsNullOrWhiteSpace(lastName))
                query = query.Where(x => x.LastName.Contains(lastName));

            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(x => x.Email.Contains(email));

            if (!string.IsNullOrWhiteSpace(userName))
                query = query.Where(x => x.UserName.Contains(userName));

            var num = await query.CountAsync();

            query = query.Skip(skip);
            if (take > 0)
                query = query.Take(take);

            var results = await query.ProjectTo<UpdateUserModel>().ToListAsync();

            return new UsersListModel
            {
                Users = results,
                NumResults = num
            };
        }

        public async Task<UpdateUserModel> GetById(string id)
        {
            var user = await _db.Users.Include("Claims").FirstOrDefaultAsync(x => x.Id == id);

            var userModel = Mapper.Map<UpdateUserModel>(user);
            userModel.Roles = user.GetUserRoles();

            return userModel;
        }

        public async Task<IdentityResult> CreateUser(UpdateUserModel model)
        {

            var user = new ApplicationUser();
            Mapper.Map(model, user);

            return await _userManager.CreateAsync(user, model.Password);


        }

        public async Task<IdentityResult> SaveUserPermissions(string id, Dictionary<string, AccessRight> roles)
        {
            var rolesStr = JsonConvert.SerializeObject(roles);
            var claims = await _userManager.GetClaimsAsync(id);
            var rolesClaims = claims.FirstOrDefault(c => c.Type == AppAuthConfiguration.Config.RolesClaimName);
            if (rolesClaims != null)
                await _userManager.RemoveClaimAsync(id, rolesClaims);

            var newRolesClaims = new Claim(AppAuthConfiguration.Config.RolesClaimName, rolesStr);


            return await _userManager.AddClaimAsync(id, newRolesClaims);
        }

        public async Task<IdentityResult> ForceUserToReload(string id, bool shouldsignout = true)
        {
            var user = await _userManager.FindByIdAsync(id);

            user.ShouldBeSignedOut = shouldsignout;
            return await _userManager.UpdateAsync(user);
        }

        public Task<IdentityResult> SignOut()
        {
            try
            {
                _authenticationManager.SignOut();
                return Task.FromResult<IdentityResult>(IdentityResult.Success);
            }
            catch (Exception ex)
            {
                return Task.FromResult<IdentityResult>(IdentityResult.Failed(ex.Message));
            }

        }
    }
}
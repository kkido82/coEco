using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using CoEco.BO.Models;
using CoEco.BO.Services;
using CoEco.BO.Auth;
using CoEco.Core.Services;
namespace CoEco.BO.Controllers
{
    [RoutePrefix("api/useradmin")]
    [RoleClaimAuthorization("usersAdmin")]
    public class UserAdminController : BaseApiController
    {

        private readonly IAuthRepository _authRepository;



        public UserAdminController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;

        }


        [HttpGet]
        public async Task<IHttpActionResult> Users([FromUri]UsersFilterModel model)
        {
            return Ok(await _authRepository.List(model.FirstName, model.LastName, model.Email, model.UserName, model.Skip, model.Take));
        }

        [HttpGet]
        [Route("getbyid/{id}")]
        public async Task<IHttpActionResult> GetById(string id)
        {
            var user = await _authRepository.GetById(id);
            if (user != null)
                return Ok(user);
            return NotFound();
        }

        [HttpPost]
        [Route("save")]
        public async Task<UpdateUserResult> SaveUser(UpdateUserModel model)
        {
            var result = new UpdateUserResult();

            if (ModelState.IsValid)
            {
                var isNew = string.IsNullOrWhiteSpace(model.Id);
                IdentityResult saveResult;
                if (!isNew)
                    saveResult = await _authRepository.UpdateUser(model);
                else
                {
                    saveResult = await _authRepository.CreateUser(model);
                }
                result.IsSuccess = saveResult.Succeeded;

                if (!result.IsSuccess)
                    result.Errors.AddRange(saveResult.Errors);

            }
            else
            {
                var errors = ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage).ToList();
                result.Errors.AddRange(errors);
            }
            return result;
        }

        [HttpPost]
        [Route("savepermissions/{id}")]
        public async Task<IHttpActionResult> SavePermissions(string id, [FromBody] Dictionary<string, AccessRight> roles)
        {
            try
            {               
                var model = new UpdateUserResult();
                if (ModelState.IsValid)
                {
                    var result = await _authRepository.SaveUserPermissions(id, roles);
                    model.IsSuccess = result.Succeeded;
                    if (!result.Succeeded)
                        model.Errors.AddRange(result.Errors);
                    else
                        await _authRepository.ForceUserToReload(id);
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage).ToList();
                    model.Errors.AddRange(errors);
                }
                return Ok(model);
            }
            catch (Exception ex)
            {
                return OnError(ex);
            }

        }

        [HttpPost]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authRepository.ChangePassword(model.UserId, model.Password);
                if (result.Succeeded)
                    return Ok();

                return GetErrorResult(result);
            }

            return BadRequest(ModelState);
        }
    }
}
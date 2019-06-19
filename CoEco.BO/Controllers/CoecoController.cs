using Breeze.ContextProvider.EF6;
using CoEco.BO.Formatters;
using CoEco.Data;
using CoEco.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Breeze.ContextProvider;
using Newtonsoft.Json.Linq;
using CoEco.BO.Models;
using CoEco.BO.Services;
using System.Threading.Tasks;

namespace CoEco.BO.Controllers
{
    [CoecoBreezeController]
    public class CoecoController : BaseApiController
    {
        private readonly EFContextProvider<CoEcoEntities> _contextProvider;
        private readonly IDataAccessService _dataAccessService;
        private readonly DistanceService distanceService;

        public CoecoController(EFContextProvider<CoEcoEntities> contextProvider,
           IDataAccessService dataAccessService,
           DistanceService distanceService)
        {

            _dataAccessService = dataAccessService;
            this.distanceService = distanceService;
            _contextProvider = contextProvider;
        }

        [HttpGet]
        public string Metadata()
        {
            return _contextProvider.Metadata();
        }

        [HttpPost]
        public SaveResult SaveChanges(JObject saveBundle)
        {
            return _contextProvider.SaveChanges(saveBundle);
        }

        [HttpGet]
        public IQueryable<Item> Items()
        {
            return _contextProvider.Context.Items;
        }

        [HttpGet]
        public IQueryable<IconStore> IconStores()
        {
            return _contextProvider.Context.IconStores;
        }
        [HttpGet]
        public IQueryable<Unit> Units()
        {
            return _contextProvider.Context.Units;
        }
        [HttpGet]
        public IQueryable<DistanceUnit> DistanceUnits()
        {
            return _contextProvider.Context.DistanceUnits;
        }
        [HttpPost]
        public async Task<IHttpActionResult> DistanceUnits(UnitDistanceModel[] models)
        {
            var res = await distanceService.Update(models);

            if (res.Success)
                return Ok();

            return BadRequest(res.Error.Description);
        }
        [HttpGet]
        public IQueryable<RatingQuestion> RatingQuestions()
        {
            return _contextProvider.Context.RatingQuestions.Where(x => !x.Disable);
        }
        [HttpGet]
        public IQueryable<Notification> Notifications()
        {
            return _contextProvider.Context.Notifications;
        }
        [HttpGet]
        public IQueryable<PermissionsProfile> PermissionsProfiles()
        {
            return _contextProvider.Context.PermissionsProfiles.Where(x => !x.Disable);
        }
        [HttpGet]
        public IQueryable<Member> Members()
        {
            return _contextProvider.Context.Members;
        }
        [HttpGet]
        public IQueryable<OrderDataReport> OrderDataReports()
        {
            return _contextProvider.Context.OrderDataReports;
        }
        [HttpPost]
        public void RemoveAspNetUser(object userDetails)
        {
            dynamic miObj = JObject.Parse(userDetails.ToString());
            string userId = miObj.userId;
            AspNetUser aspNetUser = _dataAccessService.GetAspNetUserById(userId);
            List<AspNetUserClaim> aspNetUserClaims = _dataAccessService.GetAspNetUserClaimsByUserId(userId).ToList();
            if (aspNetUserClaims.Any())
            {
                _dataAccessService.RemoveAspNetUserClaim(aspNetUserClaims);
            }
            if (aspNetUser != null)
            {
                _dataAccessService.RemoveAspNetUser(aspNetUser);
            }
        }
    }
}
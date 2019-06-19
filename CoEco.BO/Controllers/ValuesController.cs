using Breeze.ContextProvider;
using Breeze.ContextProvider.EF6;
using CoEco.BO.Formatters;
using CoEco.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace CoEco.BO.Controllers
{
    [CoecoBreezeController]
    public class ValuesController : BaseApiController
    {
        protected readonly EFContextProvider<CoEcoEntities> _contextProvider;
        public ValuesController(EFContextProvider<CoEcoEntities> contextProvider)
        {
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
        public IQueryable<IconStore> IconStores()
        {
            return _contextProvider.Context.IconStores.Where(x => !x.Disable);
        }
        [HttpGet]
        public IQueryable<Unit> Units()
        {
            return _contextProvider.Context.Units.Where(x => !x.Disable);
        }
        [HttpGet]
        public IQueryable<PermissionsProfile> PermissionsProfiles()
        {
            return _contextProvider.Context.PermissionsProfiles.Where(x => !x.Disable);
        }

        [HttpGet]
        public IQueryable<OrderStatus> OrderStatuses()
        {
            return _contextProvider.Context.OrderStatuses.Where(x => !x.Disable);
        }



    }
}
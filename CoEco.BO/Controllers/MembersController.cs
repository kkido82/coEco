using Breeze.ContextProvider.EF6;
using CoEco.BO.Auth;
using CoEco.BO.Formatters;
using CoEco.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace CoEco.BO.Controllers
{
    [CoecoBreezeController]
    [RoleClaimAuthorization("members")]
    [RoutePrefix("api/members")]
    public class MembersController: BaseApiController
    {
        private readonly EFContextProvider<CoEcoEntities> _contextProvider;

        public MembersController(EFContextProvider<CoEcoEntities> contextProvider)
        {
            _contextProvider = contextProvider;
        }


    }
}
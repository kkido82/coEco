using CoEco.Core.Context;
using CoEco.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoEco.BO.Provider
{
    public class CoEcoEFContextProvider : CoEcoContextProvider<CoEcoEntities>
    {
        private readonly IUserContext _userContext;
        public CoEcoEFContextProvider(IUserContext userContext)
        {
            _userContext = userContext;
        }
        protected override CoEcoEntities CreateContext()
        {
            return new CoEcoEntities(_userContext);
        }
    }
}
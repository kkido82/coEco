using Breeze.ContextProvider;
using Breeze.ContextProvider.EF6;
using CoEco.BO.Config;
using CoEco.BO.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace CoEco.BO.Models
{
    public static class Extensions
    {
        private static Dictionary<string, AccessRight> GetUserRoles(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return new Dictionary<string, AccessRight>();
            try
            {
                return
                    JsonConvert.DeserializeObject<Dictionary<string, AccessRight>>(str);


            }
            catch
            {
                return str.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .ToDictionary(x => x.Trim(), x => AccessRight.Write);
            }
        }

        public static Dictionary<string, AccessRight> GetUserRoles(this ClaimsIdentity identity)

        {
            if (identity != null && identity.Claims.Any(x => x.Type == AppAuthConfiguration.Config.RolesClaimName))
            {
                return
                    identity.Claims.First(x => x.Type == AppAuthConfiguration.Config.RolesClaimName)
                        .Value.GetUserRoles();
            }
            return new Dictionary<string, AccessRight>();
        }

        public static Dictionary<string, AccessRight> GetUserRoles(this ApplicationUser user)
        {
            var claimRecord = user.Claims.FirstOrDefault(x => x.ClaimType == AppAuthConfiguration.Config.RolesClaimName);
            if (claimRecord != null)
                return claimRecord.ClaimValue.GetUserRoles();

            return new Dictionary<string, AccessRight>();
        }

        public static EntityState ToCoecoEntityState(
           this Breeze.ContextProvider.EntityState state)
        {
            return (EntityState)((int)state);

        }

        //public static IEnumerable<EFEntityError> GetErrors(this IEnumerable<EntityInfo> items, IValidator validator)
        //{
        //    return items.SelectMany(entityInfo =>
        //    {
        //        var result =
        //            validator.Validate(
        //                entityInfo.Entity.MakeAuditedEntity(entityInfo.EntityState.ToMofetEntityState()));

        //        return
        //            result.Errors.Select(
        //                e => new EFEntityError(entityInfo, e.PropertyName, e.ErrorMessage, e.PropertyName));
        //    });

        //}

    }
}
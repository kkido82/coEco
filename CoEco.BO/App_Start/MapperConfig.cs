using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using CoEco.BO.Auth;
using CoEco.BO.Config;
using CoEco.BO.Models;

namespace CoEco.BO.App_Start
{
    public class MapperConfig
    {
        public static void RegisterMaps()
        {
            Mapper.Initialize(ConfigMaps);
        }

        private static void ConfigMaps(IMapperConfigurationExpression mapper)
        {
            mapper.CreateMap<UpdateUserModel, ApplicationUser>()
                .ForMember(x => x.Id,
                    mo => mo.ResolveUsing(src => string.IsNullOrWhiteSpace(src.Id) ? Guid.NewGuid().ToString() : src.Id))
                .ForMember(x => x.Roles, mo => mo.Ignore())
                .ForMember(x => x.AccountExpiresAt, mo => mo.MapFrom(x => x.AccountExpiresAt.HasValue ? new DateTime?(x.AccountExpiresAt.Value.ToLocalTime()) : null));
            mapper.CreateMap<ApplicationUser, UpdateUserModel>()
                .ForMember(x => x.Roles, mo => mo.Ignore());
        }
    }
}
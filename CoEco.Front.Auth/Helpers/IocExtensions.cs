using Autofac;
using Autofac.Core;
using CoEco.Core.Services;
using CoEco.Front.Auth.Data;
using CoEco.Front.Auth.Domain;
using System;
using System.Data.Entity;
using System.Reflection;

namespace CoEco.Front.Auth.Helpers
{
    public static class IocExtensions
    {
        static readonly AppDomainTypeFinder typeFinder = new AppDomainTypeFinder();
        static readonly Type iRepoType = typeof(IRepository<>);
        static readonly Type efRepoType = typeof(EfRepository<>);

        public static void RegisterRepositories<TDbContent>(this ContainerBuilder builder, Assembly assembly)
            where TDbContent : DbContext
        {

            var dbSetParam = new ResolvedParameter(
                (p, c) =>
                {
                    return p.ParameterType == typeof(DbContext);
                },
                (p, c) =>
                {
                    return c.Resolve<TDbContent>();
                }
            );
            var baseEntitys = typeFinder.FindClassesOfType<BaseEntity>(new[] { assembly });
            foreach (var baseEntity in baseEntitys)
            {
                var entityIRepoType = iRepoType.MakeGenericType(baseEntity);
                var entityRepoType = efRepoType.MakeGenericType(baseEntity);
                builder
                    .RegisterType(entityRepoType)
                    .As(entityIRepoType)
                    .WithParameter(dbSetParam)
                    .InstancePerLifetimeScope();
            }
        }

        public static void RegisterRepositories<TDbContent>(this ContainerBuilder builder)
            where TDbContent : DbContext
        {
            var assembly = typeof(TDbContent).Assembly;
            RegisterRepositories<TDbContent>(builder, assembly);
        }

        public static void RegisterDbContext<TDbContent>(this ContainerBuilder builder, Assembly assembly = null)
            where TDbContent : DbContext
        {
            var reg = builder.RegisterType<TDbContent>().AsSelf();

            reg.InstancePerLifetimeScope();
            if (assembly == null)
            {
                builder.RegisterRepositories<TDbContent>();
            }
            else
            {
                builder.RegisterRepositories<TDbContent>(assembly);
            }

        }

    }
}

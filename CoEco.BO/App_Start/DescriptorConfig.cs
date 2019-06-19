using CoEco.Core.Services;
using CoEco.Data.EntityTypes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace CoEco.BO.App_Start
{
    public class DescriptorConfig
    {
        public static void AddTypeDescriptors()
        {
            var typeFinder = new AppDomainTypeFinder();
            var types =
                typeFinder.FindClassesOfType<IBaseEntity>()
                    .Where(x => x.GetCustomAttribute<MetadataTypeAttribute>() != null).ToList();

            types.AddRange(typeFinder.FindClassesOfType<BaseEntity>()
                    .Where(x => x.GetCustomAttribute<MetadataTypeAttribute>() != null));

            foreach (var type in types)
            {
                TypeDescriptor.AddProvider(new AssociatedMetadataTypeTypeDescriptionProvider(type), type);

            }
        }
    }
}
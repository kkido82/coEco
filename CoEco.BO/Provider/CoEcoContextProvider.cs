using Breeze.ContextProvider;
using Breeze.ContextProvider.EF6;
using CoEco.BO.Events;
using CoEco.Core.Eventing;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;

namespace CoEco.BO.Provider
{
    public class CoEcoContextProvider<T> : EFContextProvider<T> where T : class, new()
    {
        public CoEcoContextProvider() {}
        public IEventPublisher EventPublisher { get; set; }
        protected override string BuildJsonMetadata()
        {
            //try
            //{
            JObject metadata = JObject.Parse(base.BuildJsonMetadata());

            string EFNameSpace = metadata["schema"]["namespace"].ToString();
            string typeNameSpaces = metadata["schema"]["cSpaceOSpaceMapping"].ToString();
            typeNameSpaces = "{" +
                             typeNameSpaces.Replace("],[", "]|[")
                                 .Replace("[", "")
                                 .Replace("]", "")
                                 .Replace(",", ":")
                                 .Replace("|", ",") + "}";
            JObject jTypeNameSpaces = JObject.Parse(typeNameSpaces);

            foreach (var entityType in metadata["schema"]["entityType"])
            {
                string typeName = entityType["name"].ToString();
                string defaultTypeNameSpace = EFNameSpace + "." + typeName;
                string entityTypeNameSpace = jTypeNameSpaces[defaultTypeNameSpace].ToString();
                Type t = BuildManager.GetType(entityTypeNameSpace, false);
                var metaAttribute = t.GetCustomAttribute<MetadataTypeAttribute>();
                if (metaAttribute != null)
                {
                    t = metaAttribute.MetadataClassType;
                }

                IEnumerable<JToken> metaProps = null;
                if (entityType["property"].Type == JTokenType.Object)
                    metaProps = new[] { entityType["property"] };
                else
                    metaProps = entityType["property"].AsEnumerable();

                var props = from p in metaProps
                            let pname = p["name"].ToString()
                            let prop = t.GetProperties().SingleOrDefault(prop => prop.Name == pname)
                            where prop != null
                            from attr in prop.CustomAttributes
                            where typeof(DisplayAttribute).IsAssignableFrom(attr.AttributeType)
                            select new
                            {
                                Prop = p,
                                DisplayName = (prop.GetCustomAttribute<DisplayAttribute>()).GetName()
                            };
                foreach (var p in props)
                {
                    p.Prop["displayName"] = p.DisplayName;
                }
            }

            return metadata.ToString();
            //}
            //catch (Exception)
            //{
            //    return null;
            //}

        }
        protected override void AfterSaveEntities(Dictionary<Type, List<EntityInfo>> saveMap, List<KeyMapping> keyMappings)
        {
            base.AfterSaveEntities(saveMap, keyMappings);

            Publish(saveMap);
        }

        private void Publish(Dictionary<Type, List<EntityInfo>> saveMap)
        {
            if (EventPublisher == null)
                return;
            foreach (var entity in saveMap.SelectMany(t => saveMap[t.Key]))
            {
                switch (entity.EntityState)
                {
                    case EntityState.Added:
                        EventPublisher?.Publish(new EntityAdded(entity));
                        break;
                    case EntityState.Deleted:
                        EventPublisher?.Publish(new EntityDeleted(entity));
                        break;
                    case EntityState.Modified:
                        EventPublisher?.Publish(new EntityUpdated(entity));
                        break;
                }

            }
        }
    }
}
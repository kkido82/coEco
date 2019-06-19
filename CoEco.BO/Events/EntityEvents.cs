using Breeze.ContextProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoEco.BO.Events
{
    public class BaseEntityEvent
    {
        public EntityInfo EntityInfo { get; set; }

        public BaseEntityEvent(EntityInfo entityInfo)
        {
            EntityInfo = entityInfo;
        }
    }

    public class EntityEvents : BaseEntityEvent
    {
        public EntityEvents(EntityInfo entityInfo) : base(entityInfo)
        {
        }
    }
    public class EntityAdded : BaseEntityEvent
    {
        public EntityAdded(EntityInfo entityInfo) : base(entityInfo)
        {
        }
    }

    public class EntityUpdated : BaseEntityEvent
    {
        public EntityUpdated(EntityInfo entityInfo) : base(entityInfo)
        {
        }
    }

    public class EntityDeleted : BaseEntityEvent
    {
        public EntityDeleted(EntityInfo entityInfo) : base(entityInfo)
        {
        }
    }
}
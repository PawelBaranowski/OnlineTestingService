using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineTestingService.BusinessLogic.Entities
{
    /// <summary>
    /// Base class for all entities that have a Guid as primary key.
    /// </summary>
    public abstract class EntityWithGuid<T> : Entity<T> where T : EntityWithGuid<T>, new()
    {
        public virtual Guid Guid { get; set; }
    }
}

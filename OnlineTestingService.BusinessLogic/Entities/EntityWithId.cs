using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineTestingService.BusinessLogic.Entities
{
    /// <summary>
    /// Base class for entities that have a Guid as primary key.
    /// </summary>
    public abstract class EntityWithId<T> : Entity<T> where T : EntityWithId<T>, new()
    {
        public virtual int Id { get; private set; }
    }
}

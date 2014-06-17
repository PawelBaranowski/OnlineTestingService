using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineTestingService.BusinessLogic.Entities
{
    /// <summary>
    /// Base class for entities that have a Guid as primary key.
    /// </summary>
    public abstract class EntityWithId : Entity
    {
        public virtual int Id { get; private set; }
    }
}

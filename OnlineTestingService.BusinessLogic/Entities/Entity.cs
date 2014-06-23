using System;

namespace OnlineTestingService.BusinessLogic.Entities
{
    /// <summary>
    /// Base class for all business entities.
    /// </summary>
    public abstract class Entity<T> where T : Entity<T>, new()
    {
    }
}

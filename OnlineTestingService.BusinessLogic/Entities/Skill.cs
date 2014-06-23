using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineTestingService.BusinessLogic.Entities
{
    public class Skill : EntityWithId<Skill>, EntityWithText
    {
        public virtual string Name { get; set; }
    }
}

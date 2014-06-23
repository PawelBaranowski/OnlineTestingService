using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineTestingService.BusinessLogic.Entities
{
    public class ScheduleItem : EntityWithId<ScheduleItem>
    {
        public virtual DateTime Date { get; set; }
        public virtual bool IsDone { get; set; }
        public virtual Candidate Candidate { get; set; }
        public virtual User Host { get; set; }
        public virtual Test Test { get; set; }
        public virtual string Description { get; set; }
    }
}

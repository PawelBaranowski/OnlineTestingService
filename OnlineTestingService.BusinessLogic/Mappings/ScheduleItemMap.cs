using FluentNHibernate.Mapping;
using OnlineTestingService.BusinessLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineTestingService.BusinessLogic.Mappings
{
    public class ScheduleItemMap : ClassMap<ScheduleItem>
    {
        public ScheduleItemMap()
        {
            Id(x => x.Id);
            Map(x => x.Date);
            Map(x => x.IsDone);
            References(x => x.Candidate);
            References(x => x.Host);
            References(x => x.Test);
            Map(x => x.Description);
        }
    }
}

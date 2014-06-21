using FluentNHibernate.Mapping;
using OnlineTestingService.BusinessLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineTestingService.BusinessLogic.Mappings
{
    public class SkillMap : ClassMap<Skill>
    {
        public SkillMap()
        {
            Id(x => x.Id);
            Map(x => x.Name).Not.Nullable();
        }
    }
}

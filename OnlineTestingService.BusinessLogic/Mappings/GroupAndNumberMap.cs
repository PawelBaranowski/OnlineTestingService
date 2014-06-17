using System;
using FluentNHibernate.Mapping;
using OnlineTestingService.BusinessLogic.Entities;

namespace OnlineTestingService.BusinessLogic.Mappings
{
    public class GroupAndNumberMap : ClassMap<GroupAndNumber>
    {
        public GroupAndNumberMap()
        {
            Id(x => x.Id);
            References<QuestionGroup>(x => x.Group)
                .Not.Nullable()
                .Cascade.SaveUpdate();
            Map(x => x.Number)
                .Not.Nullable();
        }
    }
}

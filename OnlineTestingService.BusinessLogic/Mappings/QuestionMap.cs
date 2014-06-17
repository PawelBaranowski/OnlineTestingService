using System;
using FluentNHibernate.Mapping;
using OnlineTestingService.BusinessLogic.Entities;

namespace OnlineTestingService.BusinessLogic.Mappings
{
    public class QuestionMap : ClassMap<Question>
    {
        public QuestionMap()
        {
            Id(x => x.Id);
            References<QuestionContent>(x => x.QuestionContent)
                .Not.Nullable();
            Component(x => x.Answer);
        }
    }
}
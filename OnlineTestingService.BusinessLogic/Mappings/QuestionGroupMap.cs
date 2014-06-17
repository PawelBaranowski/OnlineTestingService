using System;
using FluentNHibernate.Mapping;
using OnlineTestingService.BusinessLogic.Entities;
using System.Collections.Generic;

namespace OnlineTestingService.BusinessLogic.Mappings
{
    public class QuestionGroupMap : ClassMap<QuestionGroup>
    {
        public QuestionGroupMap()
        {
            Id(x => x.Id);
            Map(x => x.Name)
                .Unique()
                .Not.Nullable();
            HasManyToMany<QuestionContent>(x => x.QuestionContents)
                .Access.CamelCaseField()
                .Cascade.SaveUpdate()
                .Table("Questions_Groups");
        }
    }
}
using System;
using FluentNHibernate.Mapping;
using OnlineTestingService.BusinessLogic.Entities;

namespace OnlineTestingService.BusinessLogic.Mappings
{
    public class QuestionContentMap : ClassMap<QuestionContent>
    {
        public QuestionContentMap()
        {
            Id(x => x.Id);
            Map(x => x.Mandatory)
                .Not.Nullable();
            Map(x => x.IsDepricated)
                .Not.Nullable();
            Map(x => x.Content)
                .Not.Nullable()
                .CustomSqlType("VARCHAR(MAX)");
            Map(x => x.Time)
                .Not.Nullable();
            HasManyToMany<QuestionGroup>(x => x.InGroups)
                .Access.CamelCaseField()
                .Cascade.SaveUpdate()
                .Inverse()
                .Table("Questions_Groups");
        }
    }
}
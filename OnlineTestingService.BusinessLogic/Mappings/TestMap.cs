using System;
using FluentNHibernate.Mapping;
using OnlineTestingService.BusinessLogic.Entities;

namespace OnlineTestingService.BusinessLogic.Mappings
{
    public class TestMap : ClassMap<Test>
    {
        public TestMap()
        {
            Id(x => x.Guid)
                .GeneratedBy.Guid();
            Map(x => x.StartTime);
            Map(x => x.EndTime)
                .Not.Nullable();
            Map(x => x.Duration)
                .Not.Nullable();
            Map(x => x.IsFinished)
                .Not.Nullable();
            Map(x => x.IsReviewed)
                .Not.Nullable();
            Map(x => x.PasswordHash)
                .Not.Nullable();
            References<Candidate>(x => x.Candidate)
                .Not.Nullable();
            References<TestTemplate>(x => x.TestTemplate)
                .Not.Nullable();
            HasMany<Question>(x => x.Questions)
                .Not.LazyLoad()
                .Access.CamelCaseField()
                .Table("Tests_Questions")
                .Cascade.SaveUpdate();
        }
    }
}
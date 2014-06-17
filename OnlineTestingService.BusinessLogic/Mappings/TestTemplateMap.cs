using System;
using FluentNHibernate.Mapping;
using OnlineTestingService.BusinessLogic.Entities;
using FluentNHibernate;

namespace OnlineTestingService.BusinessLogic.Mappings
{
    public class TestTemplateMap : ClassMap<TestTemplate>
    {
        public TestTemplateMap()
        {
            Id(x => x.Id);
            Map(x => x.IsDeprecated)
                .Not.Nullable();
            Map(x => x.Name)
                .Unique()
                .Not.Nullable();
            References<File>(x => x.JobOffer)
                .Cascade.All();
            Map(Reveal.Member<TestTemplate>("Emails"));
            HasMany<GroupAndNumber>(Reveal.Member<TestTemplate>("GroupsAndNumbers"))
                .Cascade.All();
        }
    }
}
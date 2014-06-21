using System;
using FluentNHibernate.Mapping;
using OnlineTestingService.BusinessLogic.Entities;

namespace OnlineTestingService.BusinessLogic.Mappings
{
    public class CandidateMap : ClassMap<Candidate>
    {
        public CandidateMap()
        {
            Id(x => x.Id);
            Map(x => x.Name)
                .Not.Nullable();
            Component(x => x.EmailAddress);
            Component(x => x.PhoneNumber);
            Map(x => x.Inactive)
                .Not.Nullable();
            References<File>(x => x.CV)
                .Cascade.All();
            References(x => x.User);

            HasManyToMany(x => x.PerfectSkills).Access.Property().Cascade.SaveUpdate().Table("Candidate_PerfectSkills");
            HasManyToMany(x => x.GoodSkills).Access.Property().Cascade.SaveUpdate().Table("Candidate_GoodSkills");
            HasManyToMany(x => x.BasicSkills).Access.Property().Cascade.SaveUpdate().Table("Candidate_BasicSkills");
        }
    }
}
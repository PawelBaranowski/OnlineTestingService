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
        }
    }
}
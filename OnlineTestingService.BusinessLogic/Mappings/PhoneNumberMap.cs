using System;
using FluentNHibernate.Mapping;
using OnlineTestingService.BusinessLogic.Entities;

namespace OnlineTestingService.BusinessLogic.Mappings
{
    public class PhoneNumberMap : ComponentMap<PhoneNumber>
    {
        public PhoneNumberMap()
        {
            Map(x => x.Number);
        }
    }
}
using System;
using FluentNHibernate.Mapping;
using OnlineTestingService.BusinessLogic.Entities;

namespace OnlineTestingService.BusinessLogic.Mappings
{
    public class EmailAddressMap : ComponentMap<EmailAddress>
    {
        public EmailAddressMap()
        {
            Map(x => x.Address);
        }
    }
}
using FluentNHibernate.Mapping;
using OnlineTestingService.BusinessLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineTestingService.BusinessLogic.Mappings
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("aspnet_Users");
            Id(x => x.Guid, "UserId");
            Map(x => x.Name, "UserName");
            HasOne(x => x.Candidate).PropertyRef(x => x.User).Cascade.Delete();
        }
    }
}

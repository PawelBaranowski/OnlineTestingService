using System;
using FluentNHibernate.Mapping;
using OnlineTestingService.BusinessLogic.Entities;

namespace OnlineTestingService.BusinessLogic.Mappings
{
    public class FileMap : ClassMap<File>
    {
        public FileMap()
        {
            Id(x => x.Id);
            Map(x => x.Name)
                .Not.Nullable();
            Map(x => x.ContentType);
            Map(x => x.Content)
                .Not.Nullable()
                .Length(10240)
                .LazyLoad();
        }
    }
}
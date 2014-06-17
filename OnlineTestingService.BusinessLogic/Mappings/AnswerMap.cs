using System;
using FluentNHibernate.Mapping;
using OnlineTestingService.BusinessLogic.Entities;

namespace OnlineTestingService.BusinessLogic.Mappings
{
    public class AnswerMap : ComponentMap<Answer>
    {
        public AnswerMap()
        {
            Map(x => x.Grade);
            Map(x => x.Comment);
            Map(x => x.Content);
        }
    }
}
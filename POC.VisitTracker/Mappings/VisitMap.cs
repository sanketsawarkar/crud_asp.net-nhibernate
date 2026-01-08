using FluentNHibernate.Mapping;
using POC.VisitTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POC.VisitTracker.Mappings
{
    public class VisitMap : ClassMap<Visit>
    {
        public VisitMap() 
        {
            Table("Visits");

            Id(x=> x.Id).GeneratedBy.Identity();

            Map(x => x.PatientId).Not.Nullable().Length(50);
            Map(x => x.VisitDate).Not.Nullable();
            Map(x => x.VisitType).Length(20);
            Map(x => x.Status).Not.Nullable().Length(20);
            Map(x => x.CreatedDate).Nullable();
            Map(x => x.Notes).Length(int.MaxValue);
        }

    }
}
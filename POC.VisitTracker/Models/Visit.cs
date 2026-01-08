using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POC.VisitTracker.Models
{
    public class Visit
    {
        public virtual int Id { get; set; }
        public virtual string PatientId { get; set; }
        public virtual DateTime VisitDate { get; set; }
        public virtual string VisitType { get; set; }
        public virtual string Status { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string Notes { get; set; }
    }

}
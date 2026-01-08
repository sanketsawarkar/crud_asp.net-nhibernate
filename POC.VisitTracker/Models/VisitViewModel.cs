using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POC.VisitTracker.Models
{
    public class VisitViewModel
    {
        public List<Visit> VisitsList { get; set; }
        public Visit EditVisit { get; set; }
        public int TotalVisits { get; set; }

        public VisitViewModel()
        {
            VisitsList = new List<Visit>();
            EditVisit = new Visit();
        }
    }
}
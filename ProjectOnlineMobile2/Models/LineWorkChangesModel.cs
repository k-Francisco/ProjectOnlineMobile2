using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectOnlineMobile2.Models
{
    public class LineWorkChangesModel
    {
        public DateTime StartDate { get; set; }
        public string ActualHours { get; set; }
        public string PlannedHours { get; set; }

        public LineWorkChangesModel(DateTime startDate, string actualHours, string plannedHours) {
            this.StartDate = startDate;
            this.ActualHours = actualHours;
            this.PlannedHours = plannedHours;
        }

    }
}

using ProjectOnlineMobile2.Models.TLWM;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectOnlineMobile2.Models
{
    public class SavedTimesheetLineWork : RealmObject
    {
        public string PeriodId { get; set; }
        public string LineId { get; set; }
        public WorkResult WorkModel { get; set; }
    }
}

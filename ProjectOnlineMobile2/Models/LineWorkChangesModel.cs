using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjectOnlineMobile2.Models
{
    public class LineWorkChangesModel
    {
        [Key]
        public DateTime StartDate { get; set; }
        public string ActualHours { get; set; }
        public string PlannedHours { get; set; }
        public string PeriodId { get; set; }
        public string LineId { get; set; }

    }
}

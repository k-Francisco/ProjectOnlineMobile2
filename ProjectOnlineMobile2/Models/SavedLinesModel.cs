using LineResult = ProjectOnlineMobile2.Models.TLL.TimesheetLineResult;
using Realms;
using System.Collections.Generic;

namespace ProjectOnlineMobile2.Models
{
    public class SavedLinesModel : RealmObject
    {
        public string PeriodId { get; set; }
        public LineResult LineModel { get; set; }
    }
}

using LineResult = ProjectOnlineMobile2.Models.TLL.TimesheetLineResult;
using Realms;
using System.Collections.Generic;

namespace ProjectOnlineMobile2.Models
{
    public class SavedLinesModel : RealmObject
    {
        public string periodId { get; set; }
        public LineResult line { get; set; }
    }
}

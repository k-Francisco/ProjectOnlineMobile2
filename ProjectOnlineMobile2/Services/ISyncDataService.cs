using ProjectOnlineMobile2.Models;
using ProjectOnlineMobile2.Models.TLWM;
using ProjectOnlineMobile2.Models.PSPL;
using ProjectsResult = ProjectOnlineMobile2.Models.PSPL.Result;
using AssignmentResult = ProjectOnlineMobile2.Models.ResourceAssignmentModel.AssignmentResult;
using TimesheetPeriodsResult = ProjectOnlineMobile2.Models.TSPL.TimesheetPeriodResult;
using LineResult = ProjectOnlineMobile2.Models.TLL.TimesheetLineResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ProjectOnlineMobile2.Services
{
    public interface ISyncDataService
    {
        bool SyncProjects(ProjectServerProjectList projects, List<ProjectsResult> savedProjects, ObservableCollection<ProjectsResult> displayedProjects);
        bool SyncUserTasks(List<AssignmentResult> savedTasks, List<AssignmentResult> tasksFromServer, ObservableCollection<AssignmentResult> displayedTasks);
        bool SyncTimesheetPeriods(List<TimesheetPeriodsResult> savedPeriods, List<TimesheetPeriodsResult> periodsFromServer, ObservableCollection<TimesheetPeriodsResult> displayedPeriods);
        bool SyncTimesheetLines(List<SavedLinesModel> savedLines, List<LineResult> linesFromServer, ObservableCollection<LineResult> displayedLines, string periodId);
        bool SyncTimesheetLineWork(TimesheetLineWorkModel lineWorkModel, IOrderedEnumerable<SavedTimesheetLineWork> savedWork);
    }
}

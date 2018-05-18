using ProjectOnlineMobile2.Models;
using ProjectOnlineMobile2.Models.PSPL;
using ProjectOnlineMobile2.Models.TLWM;
using ProjectsResult = ProjectOnlineMobile2.Models.PSPL.Result;
using AssignmentResult = ProjectOnlineMobile2.Models.ResourceAssignmentModel.AssignmentResult;
using TimesheetPeriodsResult = ProjectOnlineMobile2.Models.TSPL.TimesheetPeriodResult;
using LineResult = ProjectOnlineMobile2.Models.TLL.TimesheetLineResult;
using Realms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ProjectOnlineMobile2.Services
{
    public class SyncDataService : ISyncDataService
    {
        private Realm realm { get; set; }

        public SyncDataService()
        {
            if (realm == null)
                realm = Realm.GetInstance();
        }

        public bool SyncProjects(ProjectServerProjectList projects, List<ProjectsResult> savedProjects, ObservableCollection<ProjectsResult> displayedProjects)
        {
            try
            {
                foreach (var item in savedProjects)
                {
                    var temp = projects.D.Results
                        .Where(p => p.ProjectId.Equals(item.ProjectId))
                        .FirstOrDefault();

                    if(temp == null)
                    {
                        realm.Write(()=> {
                            realm.Remove(item);
                        });
                        displayedProjects.Remove(item);
                    }
                }

                realm.Refresh();

                foreach (var item in projects.D.Results)
                {
                    var temp = savedProjects
                        .Where(p => p.ProjectId.Equals(item.ProjectId))
                        .FirstOrDefault();

                    if(temp == null)
                    {
                        realm.Write(()=> {
                            realm.Add(item);
                            displayedProjects.Add(item);
                        });
                    }
                    else
                    {
                        realm.Write(()=> {
                            temp.ProjectActualWork = item.ProjectActualWork;
                            temp.ProjectCreatedDate = item.ProjectCreatedDate;
                            temp.ProjectDescription = item.ProjectDescription;
                            temp.ProjectDuration = item.ProjectDuration;
                            temp.ProjectFinishDate = item.ProjectFinishDate;
                            temp.ProjectLastPublishedDate = item.ProjectLastPublishedDate;
                            temp.ProjectName = item.ProjectName;
                            temp.ProjectOwnerName = item.ProjectOwnerName;
                            temp.ProjectPercentCompleted = item.ProjectPercentCompleted;
                            temp.ProjectPercentWorkCompleted = item.ProjectPercentWorkCompleted;
                            temp.ProjectTitle = item.ProjectTitle;
                            temp.ProjectType = item.ProjectType;
                        });
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("SyncProjects", e.Message);
                return false;
            }
        }

        public bool SyncUserTasks(List<AssignmentResult> savedTasks, List<AssignmentResult> tasksFromServer, ObservableCollection<AssignmentResult> displayedTasks)
        {
            try
            {

                foreach (var item in savedTasks)
                {
                    var temp = tasksFromServer
                        .Where(p => p.AssignmentId.Equals(item.AssignmentId) &&
                                    p.TaskId.Equals(item.TaskId) &&
                                    p.ProjectId.Equals(item.ProjectId))
                        .FirstOrDefault();

                    if(temp == null)
                    {
                        realm.Write(()=> {
                            realm.Remove(item);
                        });
                        displayedTasks.Remove(item);
                    }
                }

                realm.Refresh();

                foreach (var item in tasksFromServer)
                {
                    var temp = savedTasks
                        .Where(p=> p.AssignmentId.Equals(item.AssignmentId) &&
                                   p.TaskId.Equals(item.TaskId) &&
                                   p.ProjectId.Equals(item.ProjectId))
                        .FirstOrDefault();

                    if(temp == null)
                    {
                        realm.Write(()=> {
                            realm.Add(item);
                            displayedTasks.Add(item);
                        });
                    }
                    else
                    {
                        realm.Write(() => {
                            temp.AssignmentActualCost = item.AssignmentActualCost;
                            temp.AssignmentActualOvertimeCost = item.AssignmentActualOvertimeCost;
                            temp.AssignmentActualOvertimeWork = item.AssignmentActualOvertimeWork;
                            temp.AssignmentActualRegularCost = item.AssignmentActualRegularCost;
                            temp.AssignmentActualRegularWork = item.AssignmentActualRegularWork;
                            temp.AssignmentActualWork = item.AssignmentActualWork;
                            temp.AssignmentACWP = item.AssignmentACWP;
                            temp.AssignmentBCWP = item.AssignmentBCWP;
                            temp.AssignmentBCWS = item.AssignmentBCWS;
                            temp.AssignmentBookingDescription = item.AssignmentBookingDescription;
                            temp.AssignmentBookingId = item.AssignmentBookingId;
                            temp.AssignmentBookingName = item.AssignmentBookingName;
                            temp.AssignmentBudgetCost = item.AssignmentBudgetCost;
                            temp.AssignmentBudgetMaterialWork = item.AssignmentBudgetMaterialWork;
                            temp.AssignmentBudgetWork = item.AssignmentBudgetWork;
                            temp.AssignmentCost = item.AssignmentCost;
                            temp.AssignmentCostVariance = item.AssignmentCostVariance;
                            temp.AssignmentCreatedDate = item.AssignmentCreatedDate;
                            temp.AssignmentCreatedRevisionCounter = item.AssignmentCreatedRevisionCounter;
                            temp.AssignmentCV = item.AssignmentCV;
                            temp.AssignmentDelay = item.AssignmentDelay;
                            temp.AssignmentFinishDate = item.AssignmentFinishDate;
                            temp.AssignmentFinishVariance = item.AssignmentFinishVariance;
                            temp.AssignmentIsOverallocated = item.AssignmentIsOverallocated;
                            temp.AssignmentIsPublished = item.AssignmentIsPublished;
                            temp.AssignmentMaterialActualWork = item.AssignmentMaterialActualWork;
                            temp.AssignmentMaterialWork = item.AssignmentMaterialWork;
                            temp.AssignmentModifiedDate = item.AssignmentModifiedDate;
                            temp.AssignmentModifiedRevisionCounter = item.AssignmentModifiedRevisionCounter;
                            temp.AssignmentOvertimeCost = item.AssignmentOvertimeCost;
                            temp.AssignmentOvertimeWork = item.AssignmentOvertimeWork;
                            temp.AssignmentPeakUnits = item.AssignmentPeakUnits;
                            temp.AssignmentPercentWorkCompleted = item.AssignmentPercentWorkCompleted;
                            temp.AssignmentRegularCost = item.AssignmentRegularCost;
                            temp.AssignmentRegularWork = item.AssignmentRegularWork;
                            temp.AssignmentRemainingCost = item.AssignmentRemainingCost;
                            temp.AssignmentRemainingOvertimeCost = item.AssignmentRemainingOvertimeCost;
                            temp.AssignmentRemainingOvertimeWork = item.AssignmentRemainingOvertimeWork;
                            temp.AssignmentRemainingRegularCost = item.AssignmentRemainingRegularCost;
                            temp.AssignmentRemainingRegularWork = item.AssignmentRemainingRegularWork;
                            temp.AssignmentRemainingWork = item.AssignmentRemainingWork;
                            temp.AssignmentResourcePlanWork = item.AssignmentResourcePlanWork;
                            temp.AssignmentResourceType = item.AssignmentResourceType;
                            temp.AssignmentStartDate = item.AssignmentStartDate;
                            temp.AssignmentStartVariance = item.AssignmentStartVariance;
                            temp.AssignmentSV = item.AssignmentSV;
                            temp.AssignmentType = item.AssignmentType;
                            temp.AssignmentVAC = item.AssignmentVAC;
                            temp.AssignmentWork = item.AssignmentWork;
                            temp.AssignmentWorkVariance = item.AssignmentWorkVariance;
                            temp.IsPublic = item.IsPublic;
                            temp.ProjectName = item.ProjectName;
                            temp.ResourceId = item.ResourceId;
                            temp.ResourceName = item.ResourceName;
                            temp.TaskIsActive = item.TaskIsActive;
                            temp.TaskName = item.TaskName;
                            temp.TimesheetClassId = item.TimesheetClassId;
                            temp.TypeDescription = item.TypeDescription;
                            temp.TypeName = item.TypeName;
                            
                        });
                    }
                }
                return true;
            }
            catch(Exception e)
            {
                Debug.WriteLine("SyncUserTasks", e.Message);
                return false;
            }
        }

        public bool SyncTimesheetPeriods(List<TimesheetPeriodsResult> savedPeriods, List<TimesheetPeriodsResult> periodsFromServer, ObservableCollection<TimesheetPeriodsResult> displayedPeriods)
        {
            try
            {

                foreach (var item in savedPeriods)
                {
                    var temp = periodsFromServer
                        .Where(p => p.Id.Equals(item.Id))
                        .FirstOrDefault();

                    if(temp == null)
                    {
                        realm.Write(()=> {
                            realm.Remove(item);
                        });
                        displayedPeriods.Remove(item);
                    }
                }

                realm.Refresh();

                foreach (var item in periodsFromServer)
                {
                    var temp = savedPeriods
                        .Where(p => p.Id.Equals(item.Id))
                        .FirstOrDefault();

                    if(temp == null)
                    {
                        realm.Write(()=> {
                            realm.Add(item);
                            displayedPeriods.Add(item);
                        });
                    }
                    else
                    {
                        realm.Write(() => {
                            temp.End = item.End;
                            temp.Name = item.Name;
                            temp.Start = item.Start;
                        });
                    }
                }

                return true;
            }
            catch(Exception e)
            {
                Debug.WriteLine("SyncTimesheetPeriods", e.Message);
                return false;
            }
        }

        public bool SyncTimesheetLines(List<SavedLinesModel> savedLines, List<LineResult> linesFromServer, ObservableCollection<LineResult> displayedLines, string periodId)
        {
            try
            {

                foreach (var item in linesFromServer)
                {
                    var temp = savedLines
                        .Where(p => p.LineModel.Id.Equals(item.Id))
                        .FirstOrDefault();

                    if(temp == null)
                    {
                        realm.Write(()=> {
                            realm.Add(new SavedLinesModel() {
                                LineModel = item,
                                PeriodId = periodId
                            });
                            displayedLines.Add(item);
                        });
                    }
                    else
                    {
                        realm.Write(() => {
                            temp.LineModel.Comment = item.Comment;
                            temp.LineModel.LineClass = item.LineClass;
                            temp.LineModel.ProjectName = item.ProjectName;
                            temp.LineModel.Status = item.Status;
                            temp.LineModel.TaskName = item.TaskName;
                            temp.LineModel.TotalWork = item.TotalWork;
                            temp.LineModel.TotalWorkMilliseconds = item.TotalWorkMilliseconds;
                            temp.LineModel.TotalWorkTimeSpan = item.TotalWorkTimeSpan;
                            temp.LineModel.ValidationType = item.ValidationType;
                        });
                    }
                }
                return true;
            }
            catch(Exception e)
            {
                Debug.WriteLine("SyncTimesheetLines", e.Message);
                return false;
            }
        }

        public bool SyncTimesheetLineWork(TimesheetLineWorkModel lineWorkModel, IOrderedEnumerable<SavedTimesheetLineWork> savedWork)
        {
            try
            {
                foreach (var item in savedWork)
                {
                    var temp = lineWorkModel.D.Results
                        .Where(p => p.Start.DateTime.ToShortDateString().Equals(item.WorkModel.Start.DateTime.ToShortDateString()))
                        .FirstOrDefault();

                    if(temp != null)
                    {
                        realm.Write(() => {
                            item.WorkModel.ActualWork = temp.ActualWork;
                            item.WorkModel.ActualWorkMilliseconds = temp.ActualWorkMilliseconds;
                            item.WorkModel.ActualWorkTimeSpan = temp.ActualWorkTimeSpan;
                            item.WorkModel.End = temp.End;
                            item.WorkModel.ActualWorkMilliseconds = temp.ActualWorkMilliseconds;
                            item.WorkModel.ActualWorkTimeSpan = temp.ActualWorkTimeSpan;
                            item.WorkModel.Comment = temp.Comment;
                            item.WorkModel.Id = temp.Id;
                            item.WorkModel.NonBillableOvertimeWork = temp.NonBillableOvertimeWork;
                            item.WorkModel.NonBillableOvertimeWorkMilliseconds = temp.NonBillableOvertimeWorkMilliseconds;
                            item.WorkModel.NonBillableOvertimeWorkTimeSpan = temp.NonBillableOvertimeWorkTimeSpan;
                            item.WorkModel.NonBillableWork = temp.NonBillableWork;
                            item.WorkModel.NonBillableWorkMilliseconds = temp.NonBillableWorkMilliseconds;
                            item.WorkModel.NonBillableWorkTimeSpan = temp.NonBillableWorkTimeSpan;
                            item.WorkModel.OvertimeWork = temp.OvertimeWork;
                            item.WorkModel.OvertimeWorkMilliseconds = temp.OvertimeWorkMilliseconds;
                            item.WorkModel.OvertimeWorkTimeSpan = temp.OvertimeWorkTimeSpan;
                            item.WorkModel.PlannedWork = temp.PlannedWork;
                            item.WorkModel.PlannedWorkMilliseconds = temp.PlannedWorkMilliseconds;
                            item.WorkModel.PlannedWorkTimeSpan = temp.PlannedWorkTimeSpan;
                        });
                    }
                    
                }

                return true;
            }
            catch(Exception e)
            {
                Debug.WriteLine("SyncTimesheetLineWork", e.Message);
                return false;
            }
        }
    }
}

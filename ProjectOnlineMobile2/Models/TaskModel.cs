using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectOnlineMobile2.Models.TaskModel
{
    public class TaskModel
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }
    public class D
    {
        [JsonProperty("results")]
        public List<Result> Results { get; set; }
    }
    public class Result
    {
        [JsonProperty("__metadata")]
        public Metadata Metadata { get; set; }
        [JsonProperty("CustomFields")]
        public CustomFields CustomFields { get; set; }
        [JsonProperty("SubProject")]
        public SubProject SubProject { get; set; }
        [JsonProperty("Assignments")]
        public Assignments Assignments { get; set; }
        [JsonProperty("Calendar")]
        public Calendar Calendar { get; set; }
        [JsonProperty("EntityLinks")]
        public EntityLinks EntityLinks { get; set; }
        [JsonProperty("Parent")]
        public Parent Parent { get; set; }
        [JsonProperty("Predecessors")]
        public Predecessors Predecessors { get; set; }
        [JsonProperty("StatusManager")]
        public StatusManager StatusManager { get; set; }
        [JsonProperty("Successors")]
        public Successors Successors { get; set; }
        [JsonProperty("TaskPlanLink")]
        public TaskPlanLink TaskPlanLink { get; set; }
        [JsonProperty("ActualCostWorkPerformed")]
        public int ActualCostWorkPerformed { get; set; }
        [JsonProperty("ActualDuration")]
        public string ActualDuration { get; set; }
        [JsonProperty("ActualDurationMilliseconds")]
        public int ActualDurationMilliseconds { get; set; }
        [JsonProperty("ActualDurationTimeSpan")]
        public string ActualDurationTimeSpan { get; set; }
        [JsonProperty("ActualOvertimeCost")]
        public int ActualOvertimeCost { get; set; }
        [JsonProperty("ActualOvertimeWork")]
        public string ActualOvertimeWork { get; set; }
        [JsonProperty("ActualOvertimeWorkMilliseconds")]
        public int ActualOvertimeWorkMilliseconds { get; set; }
        [JsonProperty("ActualOvertimeWorkTimeSpan")]
        public string ActualOvertimeWorkTimeSpan { get; set; }
        [JsonProperty("BaselineCost")]
        public int BaselineCost { get; set; }
        [JsonProperty("BaselineDuration")]
        public object BaselineDuration { get; set; }
        [JsonProperty("BaselineDurationMilliseconds")]
        public int BaselineDurationMilliseconds { get; set; }
        [JsonProperty("BaselineDurationTimeSpan")]
        public string BaselineDurationTimeSpan { get; set; }
        [JsonProperty("BaselineFinish")]
        public DateTime BaselineFinish { get; set; }
        [JsonProperty("BaselineStart")]
        public DateTime BaselineStart { get; set; }
        [JsonProperty("BaselineWork")]
        public object BaselineWork { get; set; }
        [JsonProperty("BaselineWorkMilliseconds")]
        public int BaselineWorkMilliseconds { get; set; }
        [JsonProperty("BaselineWorkTimeSpan")]
        public string BaselineWorkTimeSpan { get; set; }
        [JsonProperty("BudgetCost")]
        public int BudgetCost { get; set; }
        [JsonProperty("BudgetedCostWorkPerformed")]
        public int BudgetedCostWorkPerformed { get; set; }
        [JsonProperty("BudgetedCostWorkScheduled")]
        public int BudgetedCostWorkScheduled { get; set; }
        [JsonProperty("Contact")]
        public object Contact { get; set; }
        [JsonProperty("CostPerformanceIndex")]
        public int CostPerformanceIndex { get; set; }
        [JsonProperty("CostVariance")]
        public int CostVariance { get; set; }
        [JsonProperty("CostVarianceAtCompletion")]
        public int CostVarianceAtCompletion { get; set; }
        [JsonProperty("CostVariancePercentage")]
        public int CostVariancePercentage { get; set; }
        [JsonProperty("Created")]
        public DateTime Created { get; set; }
        [JsonProperty("CurrentCostVariance")]
        public int CurrentCostVariance { get; set; }
        [JsonProperty("DurationVariance")]
        public string DurationVariance { get; set; }
        [JsonProperty("DurationVarianceMilliseconds")]
        public int DurationVarianceMilliseconds { get; set; }
        [JsonProperty("DurationVarianceTimeSpan")]
        public string DurationVarianceTimeSpan { get; set; }
        [JsonProperty("EarliestFinish")]
        public DateTime EarliestFinish { get; set; }
        [JsonProperty("EarliestStart")]
        public DateTime EarliestStart { get; set; }
        [JsonProperty("EstimateAtCompletion")]
        public int EstimateAtCompletion { get; set; }
        [JsonProperty("FinishSlack")]
        public string FinishSlack { get; set; }
        [JsonProperty("FinishSlackMilliseconds")]
        public int FinishSlackMilliseconds { get; set; }
        [JsonProperty("FinishSlackTimeSpan")]
        public string FinishSlackTimeSpan { get; set; }
        [JsonProperty("FinishVariance")]
        public string FinishVariance { get; set; }
        [JsonProperty("FinishVarianceMilliseconds")]
        public int FinishVarianceMilliseconds { get; set; }
        [JsonProperty("FinishVarianceTimeSpan")]
        public string FinishVarianceTimeSpan { get; set; }
        [JsonProperty("FixedCostAccrual")]
        public int FixedCostAccrual { get; set; }
        [JsonProperty("FreeSlack")]
        public string FreeSlack { get; set; }
        [JsonProperty("FreeSlackMilliseconds")]
        public int FreeSlackMilliseconds { get; set; }
        [JsonProperty("FreeSlackTimeSpan")]
        public string FreeSlackTimeSpan { get; set; }
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("IgnoreResourceCalendar")]
        public bool IgnoreResourceCalendar { get; set; }
        [JsonProperty("IsCritical")]
        public bool IsCritical { get; set; }
        [JsonProperty("IsEffortDriven")]
        public bool IsEffortDriven { get; set; }
        [JsonProperty("IsExternalTask")]
        public bool IsExternalTask { get; set; }
        [JsonProperty("IsOverAllocated")]
        public bool IsOverAllocated { get; set; }
        [JsonProperty("IsRecurring")]
        public bool IsRecurring { get; set; }
        [JsonProperty("IsRecurringSummary")]
        public bool IsRecurringSummary { get; set; }
        [JsonProperty("IsRolledUp")]
        public bool IsRolledUp { get; set; }
        [JsonProperty("IsSubProject")]
        public bool IsSubProject { get; set; }
        [JsonProperty("IsSubProjectReadOnly")]
        public bool IsSubProjectReadOnly { get; set; }
        [JsonProperty("IsSubProjectScheduledFromFinish")]
        public bool IsSubProjectScheduledFromFinish { get; set; }
        [JsonProperty("IsSummary")]
        public bool IsSummary { get; set; }
        [JsonProperty("LatestFinish")]
        public DateTime LatestFinish { get; set; }
        [JsonProperty("LatestStart")]
        public DateTime LatestStart { get; set; }
        [JsonProperty("LevelingDelay")]
        public string LevelingDelay { get; set; }
        [JsonProperty("LevelingDelayMilliseconds")]
        public int LevelingDelayMilliseconds { get; set; }
        [JsonProperty("LevelingDelayTimeSpan")]
        public string LevelingDelayTimeSpan { get; set; }
        [JsonProperty("Modified")]
        public DateTime Modified { get; set; }
        [JsonProperty("Notes")]
        public string Notes { get; set; }
        [JsonProperty("OutlinePosition")]
        public string OutlinePosition { get; set; }
        [JsonProperty("OvertimeCost")]
        public int OvertimeCost { get; set; }
        [JsonProperty("OvertimeWork")]
        public string OvertimeWork { get; set; }
        [JsonProperty("OvertimeWorkMilliseconds")]
        public int OvertimeWorkMilliseconds { get; set; }
        [JsonProperty("OvertimeWorkTimeSpan")]
        public string OvertimeWorkTimeSpan { get; set; }
        [JsonProperty("PercentWorkComplete")]
        public int PercentWorkComplete { get; set; }
        [JsonProperty("PreLevelingFinish")]
        public DateTime PreLevelingFinish { get; set; }
        [JsonProperty("PreLevelingStart")]
        public DateTime PreLevelingStart { get; set; }
        [JsonProperty("RegularWork")]
        public string RegularWork { get; set; }
        [JsonProperty("RegularWorkMilliseconds")]
        public int RegularWorkMilliseconds { get; set; }
        [JsonProperty("RegularWorkTimeSpan")]
        public string RegularWorkTimeSpan { get; set; }
        [JsonProperty("RemainingCost")]
        public int RemainingCost { get; set; }
        [JsonProperty("RemainingOvertimeCost")]
        public int RemainingOvertimeCost { get; set; }
        [JsonProperty("RemainingOvertimeWork")]
        public string RemainingOvertimeWork { get; set; }
        [JsonProperty("RemainingOvertimeWorkMilliseconds")]
        public int RemainingOvertimeWorkMilliseconds { get; set; }
        [JsonProperty("RemainingOvertimeWorkTimeSpan")]
        public string RemainingOvertimeWorkTimeSpan { get; set; }
        [JsonProperty("RemainingWork")]
        public string RemainingWork { get; set; }
        [JsonProperty("RemainingWorkMilliseconds")]
        public int RemainingWorkMilliseconds { get; set; }
        [JsonProperty("RemainingWorkTimeSpan")]
        public string RemainingWorkTimeSpan { get; set; }
        [JsonProperty("Resume")]
        public DateTime Resume { get; set; }
        [JsonProperty("ScheduleCostVariance")]
        public int ScheduleCostVariance { get; set; }
        [JsonProperty("ScheduledDuration")]
        public string ScheduledDuration { get; set; }
        [JsonProperty("ScheduledDurationMilliseconds")]
        public int ScheduledDurationMilliseconds { get; set; }
        [JsonProperty("ScheduledDurationTimeSpan")]
        public string ScheduledDurationTimeSpan { get; set; }
        [JsonProperty("ScheduledFinish")]
        public DateTime ScheduledFinish { get; set; }
        [JsonProperty("ScheduledStart")]
        public DateTime ScheduledStart { get; set; }
        [JsonProperty("SchedulePerformanceIndex")]
        public int SchedulePerformanceIndex { get; set; }
        [JsonProperty("ScheduleVariancePercentage")]
        public int ScheduleVariancePercentage { get; set; }
        [JsonProperty("StartSlack")]
        public string StartSlack { get; set; }
        [JsonProperty("StartSlackMilliseconds")]
        public int StartSlackMilliseconds { get; set; }
        [JsonProperty("StartSlackTimeSpan")]
        public string StartSlackTimeSpan { get; set; }
        [JsonProperty("StartVariance")]
        public string StartVariance { get; set; }
        [JsonProperty("StartVarianceMilliseconds")]
        public int StartVarianceMilliseconds { get; set; }
        [JsonProperty("StartVarianceTimeSpan")]
        public string StartVarianceTimeSpan { get; set; }
        [JsonProperty("Stop")]
        public DateTime Stop { get; set; }
        [JsonProperty("ToCompletePerformanceIndex")]
        public int ToCompletePerformanceIndex { get; set; }
        [JsonProperty("TotalSlack")]
        public string TotalSlack { get; set; }
        [JsonProperty("TotalSlackMilliseconds")]
        public int TotalSlackMilliseconds { get; set; }
        [JsonProperty("TotalSlackTimeSpan")]
        public string TotalSlackTimeSpan { get; set; }
        [JsonProperty("WorkBreakdownStructure")]
        public string WorkBreakdownStructure { get; set; }
        [JsonProperty("WorkVariance")]
        public string WorkVariance { get; set; }
        [JsonProperty("WorkVarianceMilliseconds")]
        public int WorkVarianceMilliseconds { get; set; }
        [JsonProperty("WorkVarianceTimeSpan")]
        public string WorkVarianceTimeSpan { get; set; }
        [JsonProperty("ActualCost")]
        public int ActualCost { get; set; }
        [JsonProperty("ActualFinish")]
        public DateTime ActualFinish { get; set; }
        [JsonProperty("ActualStart")]
        public DateTime ActualStart { get; set; }
        [JsonProperty("ActualWork")]
        public string ActualWork { get; set; }
        [JsonProperty("ActualWorkMilliseconds")]
        public int ActualWorkMilliseconds { get; set; }
        [JsonProperty("ActualWorkTimeSpan")]
        public string ActualWorkTimeSpan { get; set; }
        [JsonProperty("BudgetWork")]
        public string BudgetWork { get; set; }
        [JsonProperty("BudgetWorkMilliseconds")]
        public int BudgetWorkMilliseconds { get; set; }
        [JsonProperty("BudgetWorkTimeSpan")]
        public string BudgetWorkTimeSpan { get; set; }
        [JsonProperty("Completion")]
        public DateTime Completion { get; set; }
        [JsonProperty("ConstraintStartEnd")]
        public DateTime ConstraintStartEnd { get; set; }
        [JsonProperty("ConstraintType")]
        public int ConstraintType { get; set; }
        [JsonProperty("Cost")]
        public int Cost { get; set; }
        [JsonProperty("Deadline")]
        public DateTime Deadline { get; set; }
        [JsonProperty("Duration")]
        public string Duration { get; set; }
        [JsonProperty("DurationMilliseconds")]
        public int DurationMilliseconds { get; set; }
        [JsonProperty("DurationTimeSpan")]
        public string DurationTimeSpan { get; set; }
        [JsonProperty("Finish")]
        public DateTime Finish { get; set; }
        [JsonProperty("FinishText")]
        public string FinishText { get; set; }
        [JsonProperty("FixedCost")]
        public int FixedCost { get; set; }
        [JsonProperty("IsActive")]
        public bool IsActive { get; set; }
        [JsonProperty("IsLockedByManager")]
        public bool IsLockedByManager { get; set; }
        [JsonProperty("IsManual")]
        public bool IsManual { get; set; }
        [JsonProperty("IsMarked")]
        public bool IsMarked { get; set; }
        [JsonProperty("IsMilestone")]
        public bool IsMilestone { get; set; }
        [JsonProperty("LevelingAdjustsAssignments")]
        public bool LevelingAdjustsAssignments { get; set; }
        [JsonProperty("LevelingCanSplit")]
        public bool LevelingCanSplit { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("OutlineLevel")]
        public int OutlineLevel { get; set; }
        [JsonProperty("PercentComplete")]
        public int PercentComplete { get; set; }
        [JsonProperty("PercentPhysicalWorkComplete")]
        public int PercentPhysicalWorkComplete { get; set; }
        [JsonProperty("Priority")]
        public int Priority { get; set; }
        [JsonProperty("RemainingDuration")]
        public string RemainingDuration { get; set; }
        [JsonProperty("RemainingDurationMilliseconds")]
        public int RemainingDurationMilliseconds { get; set; }
        [JsonProperty("RemainingDurationTimeSpan")]
        public string RemainingDurationTimeSpan { get; set; }
        [JsonProperty("Start")]
        public DateTime Start { get; set; }
        [JsonProperty("StartText")]
        public string StartText { get; set; }
        [JsonProperty("TaskType")]
        public int TaskType { get; set; }
        [JsonProperty("UsePercentPhysicalWorkComplete")]
        public bool UsePercentPhysicalWorkComplete { get; set; }
        [JsonProperty("Work")]
        public string Work { get; set; }
        [JsonProperty("WorkMilliseconds")]
        public int WorkMilliseconds { get; set; }
        [JsonProperty("WorkTimeSpan")]
        public string WorkTimeSpan { get; set; }
        [JsonProperty("Custom_x005f_0000e8d965f147699bd2819d38036fcc")]
        public CustomX005f0000e8d965f147699bd2819d38036fcc CustomX005f0000e8d965f147699bd2819d38036fcc { get; set; }
    }
    public class Metadata
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("uri")]
        public string Uri { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
    public class CustomFields
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Deferred
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
    public class SubProject
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Assignments
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Calendar
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class EntityLinks
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Parent
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Predecessors
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class StatusManager
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Successors
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class TaskPlanLink
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class CustomX005f0000e8d965f147699bd2819d38036fcc
    {
        [JsonProperty("__metadata")]
        public Metadata1 Metadata { get; set; }
        [JsonProperty("results")]
        public List<string> Results { get; set; }
    }
    public class Metadata1
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }

}

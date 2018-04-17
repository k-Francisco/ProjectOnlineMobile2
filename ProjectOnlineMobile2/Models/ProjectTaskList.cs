using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectOnlineMobile2.Models.PTL
{

    public class ProjectTaskList
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
        [JsonProperty("Assignments")]
        public Assignments Assignments { get; set; }
        [JsonProperty("AssignmentsBaselines")]
        public AssignmentsBaselines AssignmentsBaselines { get; set; }
        [JsonProperty("AssignmentsBaselineTimephasedData")]
        public AssignmentsBaselineTimephasedData AssignmentsBaselineTimephasedData { get; set; }
        [JsonProperty("Baselines")]
        public Baselines Baselines { get; set; }
        [JsonProperty("BaselinesTimephasedDataSet")]
        public BaselinesTimephasedDataSet BaselinesTimephasedDataSet { get; set; }
        [JsonProperty("Issues")]
        public Issues Issues { get; set; }
        [JsonProperty("Project")]
        public Project Project { get; set; }
        [JsonProperty("Risks")]
        public Risks Risks { get; set; }
        [JsonProperty("TimephasedInfo")]
        public TimephasedInfo TimephasedInfo { get; set; }
        [JsonProperty("ProjectId")]
        public string ProjectId { get; set; }
        [JsonProperty("TaskId")]
        public string TaskId { get; set; }
        [JsonProperty("ParentTaskId")]
        public string ParentTaskId { get; set; }
        [JsonProperty("ParentTaskName")]
        public string ParentTaskName { get; set; }
        [JsonProperty("ProjectName")]
        public string ProjectName { get; set; }
        [JsonProperty("TaskActualCost")]
        public string TaskActualCost { get; set; }
        [JsonProperty("TaskActualDuration")]
        public string TaskActualDuration { get; set; }
        [JsonProperty("TaskActualFinishDate")]
        public object TaskActualFinishDate { get; set; }
        [JsonProperty("TaskActualFixedCost")]
        public string TaskActualFixedCost { get; set; }
        [JsonProperty("TaskActualOvertimeCost")]
        public string TaskActualOvertimeCost { get; set; }
        [JsonProperty("TaskActualOvertimeWork")]
        public string TaskActualOvertimeWork { get; set; }
        [JsonProperty("TaskActualRegularCost")]
        public string TaskActualRegularCost { get; set; }
        [JsonProperty("TaskActualRegularWork")]
        public string TaskActualRegularWork { get; set; }
        [JsonProperty("TaskActualStartDate")]
        public object TaskActualStartDate { get; set; }
        [JsonProperty("TaskActualWork")]
        public string TaskActualWork { get; set; }
        [JsonProperty("TaskACWP")]
        public string TaskACWP { get; set; }
        [JsonProperty("TaskBCWP")]
        public string TaskBCWP { get; set; }
        [JsonProperty("TaskBCWS")]
        public string TaskBCWS { get; set; }
        [JsonProperty("TaskBudgetCost")]
        public string TaskBudgetCost { get; set; }
        [JsonProperty("TaskBudgetWork")]
        public string TaskBudgetWork { get; set; }
        [JsonProperty("TaskClientUniqueId")]
        public int TaskClientUniqueId { get; set; }
        [JsonProperty("TaskCost")]
        public string TaskCost { get; set; }
        [JsonProperty("TaskCostVariance")]
        public string TaskCostVariance { get; set; }
        [JsonProperty("TaskCPI")]
        public string TaskCPI { get; set; }
        [JsonProperty("TaskCreatedDate")]
        public DateTime TaskCreatedDate { get; set; }
        [JsonProperty("TaskCreatedRevisionCounter")]
        public int TaskCreatedRevisionCounter { get; set; }
        [JsonProperty("TaskCV")]
        public string TaskCV { get; set; }
        [JsonProperty("TaskCVP")]
        public string TaskCVP { get; set; }
        [JsonProperty("TaskDeadline")]
        public object TaskDeadline { get; set; }
        [JsonProperty("TaskDeliverableFinishDate")]
        public object TaskDeliverableFinishDate { get; set; }
        [JsonProperty("TaskDeliverableStartDate")]
        public object TaskDeliverableStartDate { get; set; }
        [JsonProperty("TaskDuration")]
        public string TaskDuration { get; set; }
        [JsonProperty("TaskDurationIsEstimated")]
        public bool TaskDurationIsEstimated { get; set; }
        [JsonProperty("TaskDurationString")]
        public object TaskDurationString { get; set; }
        [JsonProperty("TaskDurationVariance")]
        public string TaskDurationVariance { get; set; }
        [JsonProperty("TaskEAC")]
        public string TaskEAC { get; set; }
        [JsonProperty("TaskEarlyFinish")]
        public DateTime TaskEarlyFinish { get; set; }
        [JsonProperty("TaskEarlyStart")]
        public DateTime TaskEarlyStart { get; set; }
        [JsonProperty("TaskFinishDate")]
        public DateTime TaskFinishDate { get; set; }
        [JsonProperty("TaskFinishDateString")]
        public object TaskFinishDateString { get; set; }
        [JsonProperty("TaskFinishVariance")]
        public string TaskFinishVariance { get; set; }
        [JsonProperty("TaskFixedCost")]
        public string TaskFixedCost { get; set; }
        [JsonProperty("TaskFixedCostAssignmentId")]
        public string TaskFixedCostAssignmentId { get; set; }
        [JsonProperty("TaskFreeSlack")]
        public string TaskFreeSlack { get; set; }
        [JsonProperty("TaskHyperLinkAddress")]
        public object TaskHyperLinkAddress { get; set; }
        [JsonProperty("TaskHyperLinkFriendlyName")]
        public object TaskHyperLinkFriendlyName { get; set; }
        [JsonProperty("TaskHyperLinkSubAddress")]
        public object TaskHyperLinkSubAddress { get; set; }
        [JsonProperty("TaskIgnoresResourceCalendar")]
        public bool TaskIgnoresResourceCalendar { get; set; }
        [JsonProperty("TaskIndex")]
        public int TaskIndex { get; set; }
        [JsonProperty("TaskIsActive")]
        public bool TaskIsActive { get; set; }
        [JsonProperty("TaskIsCritical")]
        public bool TaskIsCritical { get; set; }
        [JsonProperty("TaskIsEffortDriven")]
        public bool TaskIsEffortDriven { get; set; }
        [JsonProperty("TaskIsExternal")]
        public bool TaskIsExternal { get; set; }
        [JsonProperty("TaskIsManuallyScheduled")]
        public bool TaskIsManuallyScheduled { get; set; }
        [JsonProperty("TaskIsMarked")]
        public bool TaskIsMarked { get; set; }
        [JsonProperty("TaskIsMilestone")]
        public bool TaskIsMilestone { get; set; }
        [JsonProperty("TaskIsOverallocated")]
        public bool TaskIsOverallocated { get; set; }
        [JsonProperty("TaskIsProjectSummary")]
        public bool TaskIsProjectSummary { get; set; }
        [JsonProperty("TaskIsRecurring")]
        public bool TaskIsRecurring { get; set; }
        [JsonProperty("TaskIsSummary")]
        public bool TaskIsSummary { get; set; }
        [JsonProperty("TaskLateFinish")]
        public DateTime TaskLateFinish { get; set; }
        [JsonProperty("TaskLateStart")]
        public DateTime TaskLateStart { get; set; }
        [JsonProperty("TaskLevelingDelay")]
        public string TaskLevelingDelay { get; set; }
        [JsonProperty("TaskModifiedDate")]
        public DateTime TaskModifiedDate { get; set; }
        [JsonProperty("TaskModifiedRevisionCounter")]
        public int TaskModifiedRevisionCounter { get; set; }
        [JsonProperty("TaskName")]
        public string TaskName { get; set; }
        [JsonProperty("TaskOutlineLevel")]
        public int TaskOutlineLevel { get; set; }
        [JsonProperty("TaskOutlineNumber")]
        public string TaskOutlineNumber { get; set; }
        [JsonProperty("TaskOvertimeCost")]
        public string TaskOvertimeCost { get; set; }
        [JsonProperty("TaskOvertimeWork")]
        public string TaskOvertimeWork { get; set; }
        [JsonProperty("TaskPercentCompleted")]
        public int TaskPercentCompleted { get; set; }
        [JsonProperty("TaskPercentWorkCompleted")]
        public int TaskPercentWorkCompleted { get; set; }
        [JsonProperty("TaskPhysicalPercentCompleted")]
        public int TaskPhysicalPercentCompleted { get; set; }
        [JsonProperty("TaskPriority")]
        public int TaskPriority { get; set; }
        [JsonProperty("TaskRegularCost")]
        public string TaskRegularCost { get; set; }
        [JsonProperty("TaskRegularWork")]
        public string TaskRegularWork { get; set; }
        [JsonProperty("TaskRemainingCost")]
        public string TaskRemainingCost { get; set; }
        [JsonProperty("TaskRemainingDuration")]
        public string TaskRemainingDuration { get; set; }
        [JsonProperty("TaskRemainingOvertimeCost")]
        public string TaskRemainingOvertimeCost { get; set; }
        [JsonProperty("TaskRemainingOvertimeWork")]
        public string TaskRemainingOvertimeWork { get; set; }
        [JsonProperty("TaskRemainingRegularCost")]
        public string TaskRemainingRegularCost { get; set; }
        [JsonProperty("TaskRemainingRegularWork")]
        public string TaskRemainingRegularWork { get; set; }
        [JsonProperty("TaskRemainingWork")]
        public string TaskRemainingWork { get; set; }
        [JsonProperty("TaskResourcePlanWork")]
        public string TaskResourcePlanWork { get; set; }
        [JsonProperty("TaskSPI")]
        public string TaskSPI { get; set; }
        [JsonProperty("TaskStartDate")]
        public DateTime TaskStartDate { get; set; }
        [JsonProperty("TaskStartDateString")]
        public object TaskStartDateString { get; set; }
        [JsonProperty("TaskStartVariance")]
        public string TaskStartVariance { get; set; }
        [JsonProperty("TaskStatusManagerUID")]
        public string TaskStatusManagerUID { get; set; }
        [JsonProperty("TaskSV")]
        public string TaskSV { get; set; }
        [JsonProperty("TaskSVP")]
        public string TaskSVP { get; set; }
        [JsonProperty("TaskTCPI")]
        public string TaskTCPI { get; set; }
        [JsonProperty("TaskTotalSlack")]
        public string TaskTotalSlack { get; set; }
        [JsonProperty("TaskVAC")]
        public string TaskVAC { get; set; }
        [JsonProperty("TaskWBS")]
        public string TaskWBS { get; set; }
        [JsonProperty("TaskWork")]
        public string TaskWork { get; set; }
        [JsonProperty("TaskWorkVariance")]
        public string TaskWorkVariance { get; set; }
        [JsonProperty("Health")]
        public string Health { get; set; }
        [JsonProperty("FlagStatus")]
        public object FlagStatus { get; set; }
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
    public class Assignments
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Deferred
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
    public class AssignmentsBaselines
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class AssignmentsBaselineTimephasedData
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Baselines
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class BaselinesTimephasedDataSet
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Issues
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Project
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Risks
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class TimephasedInfo
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }




}

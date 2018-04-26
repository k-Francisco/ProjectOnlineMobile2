using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectOnlineMobile2.Models.TM
{
    public class RootObject
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }
    public class D
    {
        [JsonProperty("__metadata")]
        public Metadata Metadata { get; set; }
        [JsonProperty("Creator")]
        public Creator Creator { get; set; }
        [JsonProperty("Lines")]
        public Lines Lines { get; set; }
        [JsonProperty("Manager")]
        public Manager Manager { get; set; }
        [JsonProperty("Period")]
        public Period Period { get; set; }
        [JsonProperty("Comments")]
        public string Comments { get; set; }
        [JsonProperty("EntryMode")]
        public int EntryMode { get; set; }
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("IsControlledByOwner")]
        public bool IsControlledByOwner { get; set; }
        [JsonProperty("IsProcessed")]
        public bool IsProcessed { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Status")]
        public int Status { get; set; }
        [JsonProperty("TotalActualWork")]
        public string TotalActualWork { get; set; }
        [JsonProperty("TotalActualWorkMilliseconds")]
        public int TotalActualWorkMilliseconds { get; set; }
        [JsonProperty("TotalActualWorkTimeSpan")]
        public string TotalActualWorkTimeSpan { get; set; }
        [JsonProperty("TotalNonBillableOvertimeWork")]
        public string TotalNonBillableOvertimeWork { get; set; }
        [JsonProperty("TotalNonBillableOvertimeWorkMilliseconds")]
        public int TotalNonBillableOvertimeWorkMilliseconds { get; set; }
        [JsonProperty("TotalNonBillableOvertimeWorkTimeSpan")]
        public string TotalNonBillableOvertimeWorkTimeSpan { get; set; }
        [JsonProperty("TotalNonBillableWork")]
        public string TotalNonBillableWork { get; set; }
        [JsonProperty("TotalNonBillableWorkMilliseconds")]
        public int TotalNonBillableWorkMilliseconds { get; set; }
        [JsonProperty("TotalNonBillableWorkTimeSpan")]
        public string TotalNonBillableWorkTimeSpan { get; set; }
        [JsonProperty("TotalOvertimeWork")]
        public string TotalOvertimeWork { get; set; }
        [JsonProperty("TotalOvertimeWorkMilliseconds")]
        public int TotalOvertimeWorkMilliseconds { get; set; }
        [JsonProperty("TotalOvertimeWorkTimeSpan")]
        public string TotalOvertimeWorkTimeSpan { get; set; }
        [JsonProperty("TotalWork")]
        public string TotalWork { get; set; }
        [JsonProperty("TotalWorkMilliseconds")]
        public int TotalWorkMilliseconds { get; set; }
        [JsonProperty("TotalWorkTimeSpan")]
        public string TotalWorkTimeSpan { get; set; }
        [JsonProperty("WeekStartsOn")]
        public int WeekStartsOn { get; set; }
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
    public class Creator
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Deferred
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
    public class Lines
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Manager
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Period
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }

}

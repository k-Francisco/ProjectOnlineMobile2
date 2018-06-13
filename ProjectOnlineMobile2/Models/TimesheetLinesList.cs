using Newtonsoft.Json;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectOnlineMobile2.Models.TLL
{

    public class TimesheetLinesList
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }
    public class D 
    {
        [JsonProperty("results")]
        public List<TimesheetLineResult> Results { get; set; }
    }
    public class TimesheetLineResult : RealmObject
    {
        //[JsonProperty("__metadata")]
        //public Metadata Metadata { get; set; }
        //[JsonProperty("Assignment")]
        //public Assignment Assignment { get; set; }
        //[JsonProperty("TimeSheet")]
        //public TimeSheet TimeSheet { get; set; }
        //[JsonProperty("Work")]
        //public Work Work { get; set; }
        [JsonProperty("Comment")]
        public string Comment { get; set; }
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("LineClass")]
        public int LineClass { get; set; }
        [JsonProperty("ProjectName")]
        public string ProjectName { get; set; }
        [JsonProperty("Status")]
        public int Status { get; set; }
        [JsonProperty("TaskName")]
        public string TaskName { get; set; }
        [JsonProperty("TotalWork")]
        public string TotalWork { get; set; }
        [JsonProperty("TotalWorkMilliseconds")]
        public int TotalWorkMilliseconds { get; set; }
        [JsonProperty("TotalWorkTimeSpan")]
        public string TotalWorkTimeSpan { get; set; }
        [JsonProperty("ValidationType")]
        public int ValidationType { get; set; }

        public string StatusTranslation
        {
            get {
                if (Status == 1)
                    return "Not Submitted";
                else if (Status == 5)
                    return "Awaiting approval";

                return "";
            }
        }
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
    public class Assignment
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Deferred
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
    public class TimeSheet
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Work
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }

}

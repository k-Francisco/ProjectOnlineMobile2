using Newtonsoft.Json;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectOnlineMobile2.Models.TSPL
{
    public class TimeSheetPeriodList
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }
    public class D
    {
        [JsonProperty("results")]
        public List<TimesheetPeriodResult> Results { get; set; }
    }
    public class TimesheetPeriodResult : RealmObject
    {
        //[JsonProperty("__metadata")]
        //public Metadata Metadata { get; set; }
        //[JsonProperty("TimeSheet")]
        //public TimeSheet TimeSheet { get; set; }
        [JsonProperty("End")]
        public DateTimeOffset End { get; set; }
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Start")]
        public DateTimeOffset Start { get; set; }

        public override string ToString()
        {
            return " " + Name + " ( " + Start.AddDays(1).DateTime.ToString("MM/dd/yyyy") + " - " + End.Date.ToString("MM/dd/yyyy") + " )";
        }

    }
    //public class Metadata
    //{
    //    [Key]
    //    [JsonProperty("id")]
    //    public string Id { get; set; }
    //    [JsonProperty("uri")]
    //    public string Uri { get; set; }
    //    [JsonProperty("type")]
    //    public string Type { get; set; }
    //}
    //public class TimeSheet
    //{
    //    [Key]
    //    [JsonProperty("__deferred")]
    //    public Deferred Deferred { get; set; }
    //}
    //public class Deferred
    //{
    //    [Key]
    //    [JsonProperty("uri")]
    //    public string Uri { get; set; }
    //}

}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public List<Result> Results { get; set; }
    }
    public class Result
    {
        //[JsonProperty("__metadata")]
        //public Metadata Metadata { get; set; }
        //[JsonProperty("TimeSheet")]
        //public TimeSheet TimeSheet { get; set; }
        [JsonProperty("End")]
        public DateTime End { get; set; }
        [Key]
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Start")]
        public DateTime Start { get; set; }
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

using System;

namespace TFS.RoadMap.Models
{
    public class FeatureEntity
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Group { get; set; }
        public string Risk { get; internal set; }
        public string Tags { get; internal set; }
    }
}
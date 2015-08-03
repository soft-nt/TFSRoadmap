using System.Collections.Generic;

namespace TFS.RoadMap.Models
{
    public class RoadMapEntity
    {
        public RoadMapEntity()
        {
            Features = new List<FeatureEntity>();
            Legends = new List<string>();
        }

        public string Name { get; set; }
        public List<FeatureEntity> Features { get; set; }
        public List<string> Legends { get; set; }
    }
}
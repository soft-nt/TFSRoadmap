using System.Collections.Generic;
using TFS.RoadMap.Models;

namespace TFS.RoadMap.Interfaces
{
    public interface IRoadmapProvider
    {
        IEnumerable<FeatureEntity> Get(string project, string rootQuery, string query);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TFS.RoadMap.Models;
using System.Configuration;
using TFS.RoadMap.Interfaces;
using TFS.RoadMap.Properties;

namespace TFS.RoadMap.Controllers
{
    public class RoadmapController : ApiController
    {
        private readonly IRoadmapProvider _roadmapProvider;

        public RoadmapController()
        {
            
        }

        public RoadmapController(IRoadmapProvider roadmapProvider)
        {
            _roadmapProvider = roadmapProvider;
        }

        public IEnumerable<FeatureEntity> Get(string project, string rootQuery, string query)
        {
            return _roadmapProvider.Get(project, rootQuery, query);
        }

    }
}

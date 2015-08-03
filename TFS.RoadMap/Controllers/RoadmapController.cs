using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TFS.RoadMap.Models;

namespace TFS.RoadMap.Controllers
{
    public class RoadmapController : ApiController
    {
        public IEnumerable<FeatureEntity> Get()
        {
            var tpc = new TfsTeamProjectCollection(new Uri("http://exptfs:8080/tfs/geneva"));
            var workItemStore = new WorkItemStore(tpc);

            var queryRoot = workItemStore.Projects["PSG Dashboard"].QueryHierarchy;
            var folder = (QueryFolder)queryRoot["Shared Queries"];
            var queryDef = (QueryDefinition)folder["Features for Roadmap"];

            var queryResults = workItemStore.Query(queryDef.QueryText);

            var result = new List<FeatureEntity>();

            foreach (WorkItem q in queryResults)
            {
                try
                {
                    var targetDate = (DateTime)q["Target Date"];
                    var targetEndDate = (DateTime)q["Target End Date"];

                    result.Add(new FeatureEntity
                    {
                        Start = targetDate,
                        End = targetEndDate,
                        Group = q["Group"].ToString(),
                        Title = q.Title,
                        Url = string.Format("http://exptfs:8080/tfs/Geneva/PSG%20Dashboard/_workitems#_a=edit&id={0}", q.Id),
                        Risk = q["Risk"] as string,
                        Tags = q.Tags
                    });
                }
                catch
                {
                }
            }

            return result;
        }
    }
}

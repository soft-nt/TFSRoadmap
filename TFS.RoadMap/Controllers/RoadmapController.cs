using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TFS.RoadMap.Models;
using System.Configuration;
using TFS.RoadMap.Properties;

namespace TFS.RoadMap.Controllers
{
    public class RoadmapController : ApiController
    {
        public IEnumerable<FeatureEntity> Get(string project, string rootQuery, string query)
        {
            if (string.IsNullOrEmpty(project)) throw new NullReferenceException("project");
            if (string.IsNullOrEmpty(rootQuery)) throw new NullReferenceException("rootQuery");
            if (string.IsNullOrEmpty(query)) throw new NullReferenceException("query");

            var tfsUrl = Settings.Default.TfsUrl;

            var tpc = new TfsTeamProjectCollection(new Uri(tfsUrl));
            var workItemStore = new WorkItemStore(tpc);

            var queryRoot = workItemStore.Projects[project].QueryHierarchy;
            var folder = (QueryFolder)queryRoot[rootQuery];

            QueryDefinition queryDef = null;
            foreach (var q in query.Split('/'))
	        {
                queryDef = (QueryDefinition)folder[q];
	        }

            if (queryDef == null)
	        {
		        throw new Exception(string.Format("Query {0} was not found on the root {1}", query, rootQuery));
	        }

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

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
        string TfsUrl = Settings.Default.TfsUrl;

        public IEnumerable<FeatureEntity> Get(string project, string rootQuery, string query)
        {
            if (string.IsNullOrEmpty(project)) throw new NullReferenceException("project");
            if (string.IsNullOrEmpty(rootQuery)) throw new NullReferenceException("rootQuery");
            if (string.IsNullOrEmpty(query)) throw new NullReferenceException("query");

            var tpc = new TfsTeamProjectCollection(new Uri(TfsUrl));
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

            foreach (WorkItem wi in queryResults)
            {
                try
                {
                    var targetDate = (DateTime)wi["Target Date"];
                    var targetEndDate = (DateTime)wi["Target End Date"];

                    result.Add(new FeatureEntity
                    {
                        Start = targetDate,
                        End = targetEndDate,
                        Group = wi["Group"].ToString(),
                        Title = wi.Title,
                        Url = string.Format("{0}/{1}/_workitems#_a=edit&id={2}", TfsUrl, project, wi.Id),
                        Risk = wi["Risk"] as string,
                        Tags = wi.Tags,
                        Priority = wi["Priority"].ToString(),
                        Requestor = wi["Requestor"].ToString(),
                        WIId = wi.Id,
                        ChildUSCount = GetAtivatedChildUsCount(workItemStore, wi),
                        Status = wi["Status"].ToString()
                    });
                }
                catch (Exception ex)
                {
                }
            }

            return result;
        }

        private int GetAtivatedChildUsCount(WorkItemStore workItemStore, WorkItem wi)
        {
            //TODO: To be changed to slow
            var ids = new List<int>();

            foreach (WorkItemLink item in wi.WorkItemLinks)
            {
                if (item.LinkTypeEnd.Name == "Child")
                {
                    var tWi = workItemStore.GetWorkItem(item.TargetId);
                    if (tWi.Type.Name == "User Story")
                    {
                        ids.Add(tWi.Id);
                    }
                }
            }

            var query = string.Format("SELECT [System.Id],[System.WorkItemType],[System.Title] FROM WorkItems WHERE [System.TeamProject] = 'PSG Dashboard' AND [System.WorkItemType] = 'User Story' AND [System.State] = 'Active' AND [System.Id] In ({0})", GetFormatedIds(ids));
            return workItemStore.QueryCount(query);
        }

        private string GetFormatedIds(List<int> ids)
        {
            var count = 0;
            var result = "";

            foreach (var id in ids)
            {
                if (count > 0)
                {
                    result = string.Concat(result, ",");
                }

                result = string.Concat(result, "'", id, "'");

                count++;
            }

            return result;
        }
    }
}

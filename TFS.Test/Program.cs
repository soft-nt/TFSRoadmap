using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFS.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var tpc = new TfsTeamProjectCollection(new Uri("http://exptfs:8080/tfs/geneva"));
            var workItemStore = new WorkItemStore(tpc);

            var queryRoot = workItemStore.Projects["PSG Dashboard"].QueryHierarchy;
            var folder = (QueryFolder)queryRoot["Shared Queries"];
            var queryDef = (QueryDefinition)folder["All Active Features"];
            
            var queryResults = workItemStore.Query(queryDef.QueryText);

            foreach (WorkItem q in queryResults)
            {
                Console.WriteLine(q.Tags);
            }
        }
    }
}

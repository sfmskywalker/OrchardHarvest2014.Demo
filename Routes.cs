using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace OrchardHarvest2014.WorkflowsJobsDemo {
    public class Routes : IRouteProvider {
        public IEnumerable<RouteDescriptor> GetRoutes() {
            yield return new RouteDescriptor {
                Route = new Route(
                    "MailingList/{action}",
                    new RouteValueDictionary {
                        {"action", "Index"},
                        {"controller", "MailingList"},
                        {"area", "OrchardHarvest2014.WorkflowsJobsDemo"}
                    },
                    new RouteValueDictionary(),
                    new RouteValueDictionary {
                        {"area", "OrchardHarvest2014.WorkflowsJobsDemo"}
                    },
                    new MvcRouteHandler())
            };
        }

        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (var route in GetRoutes()) {
                routes.Add(route);
            }
        }
    }
}